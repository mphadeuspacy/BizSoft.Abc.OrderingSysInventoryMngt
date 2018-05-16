using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BizSoft.Ordering.Core.Entities.Order;
using BizSoft.Ordering.Core.SeedWork.Abstracts;
using BizSoft.Ordering.EntityFrameworkCore;
using BizSoft.Ordering.EntityFrameworkCore.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

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
                (options => { options.UseSqlServer(Configuration["ConnectionString"]); }
                );

            services.AddMvc();
            // Register custom application dependencies.
            services.AddScoped<IRepository<Order>, OrderRepository>();
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
