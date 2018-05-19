using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.IO;
using BizSoft.IntegrationEventLogEf;
using BizSoft.Ordering.EntityFrameworkCore;
using BizSoft.Ordering.WebApi.Infrastructure;
using BizSoft.WebHost.Customization;

namespace BizSoft.Ordering.WebApi
{
    public class Program
    {
        public static void Main( string[] args )
        {
            BuildWebHost( args )
                .MigrateDbContext<OrderingDbContext>( ( context, services ) =>
                {
                    var env = services.GetService<IHostingEnvironment>();

                    var settings = services.GetService<IOptions<OrderingSettings>>();

                    var logger = services.GetService<ILogger<OrderingDbContextSeed>>();

                    new OrderingDbContextSeed().SeedAsync( context, env, settings, logger ).Wait();
                } )
                .MigrateDbContext<IntegrationEventLogDbContext>( ( _, __ ) => { } ).Run();
        }

        public static IWebHost BuildWebHost( string[] args ) =>
            Microsoft.AspNetCore.WebHost.CreateDefaultBuilder( args )
                .UseStartup<Startup>()
                .UseContentRoot( Directory.GetCurrentDirectory() )
                .ConfigureAppConfiguration( ( builderContext, config ) =>
                {
                    config.AddJsonFile( "settings.json" );
                    config.AddEnvironmentVariables();
                } )
                .ConfigureLogging( ( hostingContext, builder ) =>
                {
                    builder.AddConfiguration( hostingContext.Configuration.GetSection( "Logging" ) );
                    builder.AddConsole();
                    builder.AddDebug();
                } )
                .UseApplicationInsights()
                .Build();
    }
}
