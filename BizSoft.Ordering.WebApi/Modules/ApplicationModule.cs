using System.Reflection;
using Autofac;
using BizSoft.Ordering.Core.SeedWork.Abstracts;
using BizSoft.Ordering.EntityFrameworkCore.Repositories;
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
            builder.RegisterType<OrderRepository>()
                .As<IOrderRepository>()
                .InstancePerLifetimeScope();
        }
    }
}
