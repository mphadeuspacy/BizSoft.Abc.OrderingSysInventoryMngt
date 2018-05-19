using System.IO;
using Microsoft.Extensions.DependencyInjection;
using BizSoft.IntegrationEventLogEf;
using BizSoft.WebHost.Customization;
using Catalog.WebApi.Infrastructure;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Catalog.WebApi
{
    public class Program
    {
        public static void Main( string[] args )
        {
            BuildWebHost( args )
                .MigrateDbContext<CatalogDbContext>( ( context, services ) =>
                {
                    var env = services.GetService<IHostingEnvironment>();

                    var settings = services.GetService<IOptions<CatalogSettings>>();

                    var logger = services.GetService<ILogger<CatalogDbContextSeed>>();

                    new CatalogDbContextSeed()
                        .SeedAsync( context, env, settings, logger )
                        .Wait();

                } )
                .MigrateDbContext<IntegrationEventLogDbContext>( ( _, __ ) => { } )
                .Run();
        }

        private static IWebHost BuildWebHost( string[] args ) =>
            WebHost.CreateDefaultBuilder( args )
                .UseStartup<Startup>()
                .UseContentRoot( Directory.GetCurrentDirectory() )
                .UseWebRoot( "Pics" )
                .ConfigureAppConfiguration( ( builderContext, config ) =>
                {
                    config.AddEnvironmentVariables();
                } )
                .ConfigureLogging( ( hostingContext, builder ) =>
                {
                    builder.AddConfiguration( hostingContext.Configuration.GetSection( "Logging" ) );
                    builder.AddConsole();
                    builder.AddDebug();
                } )
                .Build();
    }
}
