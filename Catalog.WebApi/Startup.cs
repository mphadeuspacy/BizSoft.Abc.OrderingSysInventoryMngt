using System;
using System.Data.Common;
using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using BizSoft.EventBus.Abstracts;
using BizSoft.EventBus.Concretes;
using BizSoft.EventBusRabbitMq.Abstracts;
using BizSoft.EventBusRabbitMq.Concretes;
using BizSoft.EventBusServiceBus.Abstracts;
using BizSoft.EventBusServiceBus.Concretes;
using BizSoft.IntegrationEventLogEf;
using BizSoft.IntegrationEventLogEf.Services.Abstracts;
using BizSoft.IntegrationEventLogEf.Services.Concretes;
using Catalog.WebApi.Filters;
using Catalog.WebApi.Infrastructure;
using Catalog.WebApi.IntegrationEvents;
using Catalog.WebApi.IntegrationEvents.EventHandling;
using Catalog.WebApi.IntegrationEvents.Events;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.ServiceBus;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace Catalog.WebApi
{
    public class Startup
    {
        public Startup( IConfiguration configuration ) => Configuration = configuration;

        public IConfiguration Configuration { get; }

        public IServiceProvider ConfigureServices( IServiceCollection services )
        {
            services.AddMvc( options =>
            {
                options.Filters.Add( typeof( HttpGlobalExceptionFilter ) );
            } ).AddControllersAsServices();

            services.AddDbContext<CatalogDbContext>( options =>
            {
                options.UseSqlServer( Configuration["ConnectionString"],
                                     sqlOptions =>
                                     {
                                         sqlOptions.MigrationsAssembly( typeof( Startup ).GetTypeInfo().Assembly.GetName().Name );
                                         sqlOptions.EnableRetryOnFailure( 10, TimeSpan.FromSeconds( 30 ), null );
                                     } );
                
                options.ConfigureWarnings( warnings => warnings.Throw( RelationalEventId.QueryClientEvaluationWarning ) );
            } );

            services.AddDbContext<IntegrationEventLogDbContext>( options =>
            {
                options.UseSqlServer( Configuration["ConnectionString"],
                                     sqlOptions =>
                                     {
                                         sqlOptions.MigrationsAssembly( typeof( Startup ).GetTypeInfo().Assembly.GetName().Name );
                                         sqlOptions.EnableRetryOnFailure( 10, TimeSpan.FromSeconds( 30 ), null );
                                     } );
            } );

            services.Configure<CatalogSettings>( Configuration );

            services.AddSwaggerGen( options =>
            {
                options.DescribeAllEnumsAsStrings();
                options.SwaggerDoc( "v1", new Swashbuckle.AspNetCore.Swagger.Info
                {
                    Title = "VirtualStore - Catalog Api",
                    Version = "v1",
                    Description = "The Catalog Microservice Http Api. This is a Data-Driven/CRUD microservice",
                    TermsOfService = "Terms Of Service"
                } );
            } );
            
            services.AddTransient<Func<DbConnection, IIntegrationEventLogService>>( sp => c => new IntegrationEventLogService( c ) );

            services.AddTransient<ICatalogIntegrationEventService, CatalogIntegrationEventService>();

            if (Configuration.GetValue<bool>( "AzureServiceBusEnabled" ))
            {
                services.AddSingleton<IServiceBusPersisterConnection>( sp =>
                {
                    var settings = sp.GetRequiredService<IOptions<CatalogSettings>>().Value;

                    var serviceBusConnection = new ServiceBusConnectionStringBuilder( settings.EventBusConnection );

                    return new DefaultServiceBusPersisterConnection( serviceBusConnection );
                } );
            }
            else
            {
                services.AddSingleton<IRabbitMqPersistentConnection>( sp =>
                {
                    var logger = sp.GetRequiredService<ILogger<DefaultRabbitMqPersistentConnection>>();

                    var factory = new ConnectionFactory
                    {
                        HostName = Configuration["EventBusConnection"]
                    };

                    if (string.IsNullOrWhiteSpace( Configuration["EventBusUserName"] ) == false) factory.UserName = Configuration["EventBusUserName"];

                    if (string.IsNullOrWhiteSpace( Configuration["EventBusPassword"] ) == false) factory.Password = Configuration["EventBusPassword"];
                    
                    var retryCount = 5;

                    if (string.IsNullOrWhiteSpace( Configuration["EventBusRetryCount"] ) == false) retryCount = int.Parse( Configuration["EventBusRetryCount"] );
                    
                    return new DefaultRabbitMqPersistentConnection( factory, logger, retryCount );
                } );
            }

            RegisterEventBus( services );

            var container = new ContainerBuilder();

            container.Populate( services );

            return new AutofacServiceProvider( container.Build() );
        }

        public void Configure( IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory )
        {
            loggerFactory.AddConsole( Configuration.GetSection( "Logging" ) );

            loggerFactory.AddDebug();

            loggerFactory.AddAzureWebAppDiagnostics();

            loggerFactory.AddApplicationInsights( app.ApplicationServices, LogLevel.Trace );

            var pathBase = Configuration["PATH_BASE"];

            if (string.IsNullOrWhiteSpace( pathBase ) == false)
            {
                loggerFactory.CreateLogger( "init" ).LogDebug( $"Using PATH BASE '{pathBase}'" );

                app.UsePathBase( pathBase );
            }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
            app.Map( "/liveness", lapp => lapp.Run( async ctx => ctx.Response.StatusCode = 200 ) );
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously

            app.UseMvcWithDefaultRoute();

            app.UseSwagger()
              .UseSwaggerUI( c =>
              {
                  c.SwaggerEndpoint( $"{ (string.IsNullOrWhiteSpace( pathBase ) == false ? pathBase : string.Empty) }/swagger/v1/swagger.json", "Catalog.Api V1" );
              } );

            ConfigureEventBus( app );
        }
        
        private void RegisterEventBus( IServiceCollection services )
        {
            var subscriptionClientName = Configuration["SubscriptionClientName"];

            if (Configuration.GetValue<bool>( "AzureServiceBusEnabled" ))
            {
                services.AddSingleton<IEventBus, EventBusServiceBus>( sp =>
                {
                    var serviceBusPersisterConnection = sp.GetRequiredService<IServiceBusPersisterConnection>();

                    var iLifetimeScope = sp.GetRequiredService<ILifetimeScope>();

                    var logger = sp.GetRequiredService<ILogger<EventBusServiceBus>>();

                    var eventBusSubcriptionsManager = sp.GetRequiredService<IEventBusSubscriptionsManager>();

                    return new EventBusServiceBus( serviceBusPersisterConnection, logger, eventBusSubcriptionsManager, subscriptionClientName, iLifetimeScope );
                } );
            }
            else
            {
                services.AddSingleton<IEventBus, EventBusRabbitMq>( sp =>
                {
                    var rabbitMqPersistentConnection = sp.GetRequiredService<IRabbitMqPersistentConnection>();

                    var iLifetimeScope = sp.GetRequiredService<ILifetimeScope>();

                    var logger = sp.GetRequiredService<ILogger<EventBusRabbitMq>>();

                    var eventBusSubcriptionsManager = sp.GetRequiredService<IEventBusSubscriptionsManager>();

                    var retryCount = 5;

                    if (string.IsNullOrWhiteSpace( Configuration["EventBusRetryCount"] ) == false) retryCount = int.Parse( Configuration["EventBusRetryCount"] );
                    
                    return new EventBusRabbitMq( rabbitMqPersistentConnection, logger, iLifetimeScope, eventBusSubcriptionsManager, subscriptionClientName, retryCount );
                } );
            }

            services.AddSingleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();

            services.AddTransient<OrderStatusChangedToAwaitingValidationIntegrationEventHandler>();

            services.AddTransient<OrderStatusChangedToPaidIntegrationEventHandler>();
        }
        protected virtual void ConfigureEventBus( IApplicationBuilder app )
        {
            var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();

            eventBus.Subscribe<OrderStatusChangedToAwaitingValidationIntegrationEvent, OrderStatusChangedToAwaitingValidationIntegrationEventHandler>();

            eventBus.Subscribe<OrderStatusChangedToPaidIntegrationEvent, OrderStatusChangedToPaidIntegrationEventHandler>();
        }
    }
}


