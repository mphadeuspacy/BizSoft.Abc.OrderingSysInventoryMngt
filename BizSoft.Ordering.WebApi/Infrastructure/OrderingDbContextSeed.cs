using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BizSoft.Ordering.Core.Entities.OrderStatus;
using BizSoft.Ordering.EntityFrameworkCore;
using BizSoft.Ordering.WebApi;
using BizSoft.Ordering.WebApi.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly;

namespace BizSoft.Ordering.WebApi.Infrastructure
{
    public class OrderingDbContextSeed
    {
        public async Task SeedAsync( OrderingDbContext context, IHostingEnvironment env, IOptions<OrderingSettings> settings, ILogger<OrderingDbContextSeed> logger )
        {
            var policy = CreatePolicy( logger, nameof( OrderingDbContextSeed ) );

            await policy.ExecuteAsync( async () =>
            {

                var useCustomizationData = settings.Value
                .UseCustomizationData;

                var contentRootPath = env.ContentRootPath;


                using (context)
                {
                    context.Database.Migrate();

                    if (!context.OrderStatus.Any())
                    {
                        context.OrderStatus.AddRange( useCustomizationData
                                                ? GetOrderStatusFromFile( contentRootPath, logger )
                                                : GetPredefinedOrderStatus() );
                    }

                    await context.SaveChangesAsync();
                }
            } );
        }

        private IEnumerable<OrderStatus> GetOrderStatusFromFile( string contentRootPath, ILogger<OrderingDbContextSeed> log )
        {
            string csvFileOrderStatus = Path.Combine( contentRootPath, "Setup", "OrderStatus.csv" );

            if (!File.Exists( csvFileOrderStatus ))
            {
                return GetPredefinedOrderStatus();
            }

            try
            {
                string[] requiredHeaders = { "OrderStatus" };
                GetHeaders( requiredHeaders, csvFileOrderStatus );
            }
            catch (Exception ex)
            {
                log.LogError( ex.Message );
                return GetPredefinedOrderStatus();
            }

            int id = 1;

            return File.ReadAllLines( csvFileOrderStatus )
                                        .Skip( 1 ) // skip header row
                                        .SelectTry( x => CreateOrderStatus( x, ref id ) )
                                        .OnCaughtException( ex => { log.LogError( ex.Message ); return null; } )
                                        .Where( x => x != null );
        }

        private OrderStatus CreateOrderStatus( string value, ref int id )
        {
            if (string.IsNullOrWhiteSpace( value ))
            {
                throw new Exception( "Orderstatus is null or empty" );
            }

            return new OrderStatus( id++, value.Trim( '"' ).Trim().ToLowerInvariant() );
        }

        private IEnumerable<OrderStatus> GetPredefinedOrderStatus()
        {
            return new List<OrderStatus>()
            {
                OrderStatus.Submitted,
                OrderStatus.AwaitingValidation,
                OrderStatus.StockConfirmed,
                OrderStatus.Paid,
                OrderStatus.Shipped,
                OrderStatus.Cancelled
            };
        }

        private string[] GetHeaders( string[] requiredHeaders, string csvfile )
        {
            string[] csvheaders = File.ReadLines( csvfile ).First().ToLowerInvariant().Split( ',' );

            if (csvheaders.Count() != requiredHeaders.Count())
            {
                throw new Exception( $"requiredHeader count '{ requiredHeaders.Count()}' is different then read header '{csvheaders.Count()}'" );
            }

            foreach (var requiredHeader in requiredHeaders)
            {
                if (!csvheaders.Contains( requiredHeader ))
                {
                    throw new Exception( $"does not contain required header '{requiredHeader}'" );
                }
            }

            return csvheaders;
        }


        private Policy CreatePolicy( ILogger<OrderingDbContextSeed> logger, string prefix, int retries = 3 )
        {
            return Policy.Handle<SqlException>().
                WaitAndRetryAsync(
                    retryCount: retries,
                    sleepDurationProvider: retry => TimeSpan.FromSeconds( 5 ),
                    onRetry: ( exception, timeSpan, retry, ctx ) =>
                    {
                        logger.LogTrace( $"[{prefix}] Exception {exception.GetType().Name} with message ${exception.Message} detected on attempt {retry} of {retries}" );
                    }
                );
        }
    }
}
