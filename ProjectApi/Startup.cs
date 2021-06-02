using ConsulExtensions;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ProjectApi.Application.Queries;
using ProjectApi.Application.Service;
using ProjectApi.Domain.AggregatesModel;
using ProjectApi.Infrastructure;
using ProjectApi.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace ProjectApi
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
            //添加DbContext依赖注入      
            services.AddDbContext<ProjectContext>(options =>
            {
                //这里是mysql数据库生成，需要安装两个包一个是：
                //Pomelo.EntityFrameworkCore.Mysql   Microsoft.EntityFrameworkCore.Tool
                //options.UseMySql(Configuration.GetConnectionString("MysqlProject"),
                //    t=> {
                //        t.MigrationsAssembly(typeof(Startup).GetTypeInfo().Assembly.GetName().Name);
                //    });

                //sqlserver数据库生成
                options.UseSqlServer(Configuration.GetConnectionString("SqlServerConnStr"), t => {
                    t.MigrationsAssembly(typeof(Startup).GetTypeInfo().Assembly.GetName().Name);
                });
            });

            ////添加Authentication配置
            ////清除默认的JwtToken默认的绑定
            //JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            //services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            //        .AddJwtBearer(option =>
            //        {
            //            option.RequireHttpsMetadata = false;
            //            option.Audience = "project_api"; //需要进行验证的 ApiResource
            //            option.Authority = "http://localhost:5000";
            //        });

            ////添加Consul服务注入
            //services.AddConsulClient(Configuration.GetSection("ServiceDiscovery"))
            //        .AddDnsClient();

            //添加服务依赖注入
            services.AddScoped<IProjectRepository, ProjectRepository>()
                .AddScoped<IProjectQueries,ProjectQueries>()
                .AddScoped<IRecommendService,TestRecommendService>();

            //添加MeditatR注入
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
