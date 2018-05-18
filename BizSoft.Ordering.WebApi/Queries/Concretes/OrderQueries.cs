using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using BizSoft.Ordering.WebApi.Queries.Abstracts;

namespace BizSoft.Ordering.WebApi.Queries.Concretes
{
    public class OrderQueries : IOrderQueries
    {
        private readonly string _connectionString;

        public OrderQueries(string connectingString)
        {
            _connectionString = !string.IsNullOrWhiteSpace( connectingString ) ? connectingString : throw new ArgumentNullException( nameof( connectingString ) );
        }

        public async Task<OrderViewModel> GetOrderAsync(int id)
        {
            using (var connection = new SqlConnection( _connectionString ))
            {
                connection.Open();

                var result = await connection.QueryAsync<dynamic>(
                    @"select o.[Id] as ordernumber,o.OrderDate as date, o.Description as description,
                        o.Address_City as city, o.Address_Country as country, o.Address_State as state, o.Address_Street as street, o.Address_ZipCode as zipcode,
                        os.Name as status, 
                        oi.ProductName as productname, oi.Units as units, oi.UnitPrice as unitprice, oi.PictureUrl as pictureurl
                        FROM ordering.Orders o
                        LEFT JOIN ordering.Orderitems oi ON o.Id = oi.orderid 
                        LEFT JOIN ordering.orderstatus os on o.OrderStatusId = os.Id
                        WHERE o.Id=@id"
                    , new { id }
                );

                if (result.AsList().Count == 0)
                    throw new KeyNotFoundException();

                return MapOrderItems( result );
            }
        }

        public async Task<IEnumerable<OrderSummaryViewModel>> GetOrdersAsync()
        {
            using (var connection = new SqlConnection( _connectionString ))
            {
                connection.Open();

                return await connection.QueryAsync<OrderSummaryViewModel>( @"SELECT o.[Id] as ordernumber,o.[OrderDate] as [date],os.[Name] as [status],SUM(oi.units*oi.unitprice) as total
                     FROM [ordering].[Orders] o
                     LEFT JOIN[ordering].[orderitems] oi ON  o.Id = oi.orderid 
                     LEFT JOIN[ordering].[orderstatus] os on o.OrderStatusId = os.Id                     
                     GROUP BY o.[Id], o.[OrderDate], os.[Name] 
                     ORDER BY o.[Id]" );
            }
        }

        private OrderViewModel MapOrderItems( dynamic result )
        {
            var order = new OrderViewModel
            {
                Ordernumber = result[0].ordernumber,
                Date = result[0].date,
                Status = result[0].status,
                Description = result[0].description,
                Street = result[0].street,
                City = result[0].city,
                Zipcode = result[0].zipcode,
                Country = result[0].country,
                OrderItems = new List<OrderItemViewModel>(),
                Total = 0
            };

            foreach (dynamic item in result)
            {
                var orderitem = new OrderItemViewModel
                {
                    Productname = item.productname,
                    NumberOfUnits = item.units,
                    Price = (double)item.unitprice,
                    ImageUri = item.pictureurl
                };

                order.Total += item.units * item.unitprice;
                order.OrderItems.Add( orderitem );
            }

            return order;
        }
    }
}
