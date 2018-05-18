using System.Reflection;
using Autofac;
using BizSoft.EventBus.Abstracts;
using BizSoft.Ordering.Core.SeedWork.Abstracts;
using BizSoft.Ordering.EntityFrameworkCore.Idempotency;
using BizSoft.Ordering.EntityFrameworkCore.Repositories;
using BizSoft.Ordering.WebApi.Queries.Abstracts;
using BizSoft.Ordering.WebApi.Queries.Concretes;
using Ordering.WebApi.Commands.Concretes;

namespace BizSoft.Ordering.WebApi.Modules
{

    /// <summary>
    ///  When using Autofac you typically register the types via modules, which allow you to split the registration types between multiple files
    ///  depending on where your types are, just as you could have the application types distributed across multiple class libraries. 
    /// </summary>
    public class ApplicationModule : Autofac.Module
    {
        public string QueriesConnectionString { get; }

        public ApplicationModule( string queriesConnectionString )
        {
            QueriesConnectionString = queriesConnectionString;
        }

        protected override void Load( ContainerBuilder builder )
        {
            builder.Register( c => new OrderQueries( QueriesConnectionString ) ).As<IOrderQueries>().InstancePerLifetimeScope();

            builder.RegisterType<BuyerRepository>().As<IBuyerRepository>().InstancePerLifetimeScope();

            builder.RegisterType<OrderRepository>().As<IOrderRepository>().InstancePerLifetimeScope();

            // Asserting only one command type handled no matter how many times it is sent
            builder.RegisterType<RequestManager>().As<IRequestManager>().InstancePerLifetimeScope();

            // This is required for consistency between different data stores
            // by broadcasting events after a command has been successfully executed.
            builder.RegisterAssemblyTypes(typeof(CreateOrderCommandHandler).GetTypeInfo().Assembly).AsClosedTypesOf(typeof(IIntegrationEventHandler<>));
        }
    }
}
