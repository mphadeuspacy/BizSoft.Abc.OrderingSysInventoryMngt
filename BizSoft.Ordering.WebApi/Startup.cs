using System;
using System.Collections.Generic;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using BizSoft.Ordering.EntityFrameworkCore;
using BizSoft.Ordering.WebApi.Modules;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Ordering.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Register out-of-the-box framework services.
            services
                .AddEntityFrameworkSqlServer()
                .AddDbContext<OrderingDbContext>
                (
                    options =>
                    {
                        options.UseSqlServer
                        (
                            Configuration["ConnectionString"],
                            sqlServerOptionsAction: sqlOptions =>
                            {
                                sqlOptions.EnableRetryOnFailure
                                (
                                    maxRetryCount: 5,
                                    maxRetryDelay: TimeSpan.FromSeconds(20),
                                    errorNumbersToAdd: null
                                );
                            }
                        );
                    }
                );

            services.AddMvc();

            // Autofac config
            var container = new ContainerBuilder();

            container.Populate( services );

            container.RegisterModule( new ApplicationModule( Configuration["ConnectionString"] ) );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
