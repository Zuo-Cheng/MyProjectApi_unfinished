﻿using ConsulExtensions.Dtos;
using Consul;
using System;
using System.Linq;
using System.Net;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using DnsClient;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Hosting.Server.Features;

namespace ConsulExtensions
{
    public static class ConsulHelper
    {
        /// <summary>
        /// 添加 ConsulClient 服务依赖，需提前添加配置ServiceDiscoveryOptions
        /// </summary>
        /// <param name="services"></param>
        /// <param name="optionAction">ServiceDiscoveryOptions 配置信息</param>
        public static IServiceCollection AddConsulClient(this IServiceCollection services,IConfigurationSection configurationSection)
        {
            //添加配置文件注入
            services.Configure<ServiceDiscoveryOptions>(configurationSection);
            
            //注入IConsulClient 用于向Consul进行注册 
            services.AddSingleton<IConsulClient>(b => new ConsulClient(cfg =>
            {
                //从依赖注入中读取 Consul 的配置信息
                var serviceConfiguration = b.GetRequiredService<IOptions<ServiceDiscoveryOptions>>().Value;
                if (!string.IsNullOrEmpty(serviceConfiguration.Consul.HttpEndpoint))
                {
                    // if not configured, the client will use the default value "127.0.0.1:8500"
                    cfg.Address = new Uri(serviceConfiguration.Consul.HttpEndpoint);
                }               
            }));
            return services;
        }

        public static IServiceCollection AddConsulClient(this IServiceCollection services, Action<ServiceDiscoveryOptions> optionAction)
        {
            //添加配置文件注入
            if (optionAction == null) throw new ArgumentNullException(nameof(optionAction));

            services.Configure(optionAction);

            //注入IConsulClient 用于向Consul进行注册 
            services.AddSingleton<IConsulClient>(b => new ConsulClient(cfg =>
            {
                //从依赖注入中读取 Consul 的配置信息
                var serviceConfiguration = b.GetRequiredService<IOptions<ServiceDiscoveryOptions>>().Value;
                if (!string.IsNullOrEmpty(serviceConfiguration.Consul.HttpEndpoint))
                {
                    // if not configured, the client will use the default value "127.0.0.1:8500"
                    cfg.Address = new Uri(serviceConfiguration.Consul.HttpEndpoint);
                }
            }));
            return services;
        }


        /// <summary>
        /// 添加DnsClient 服务依赖，需提前添加配置ServiceDiscoveryOptions
        /// </summary>
        /// <param name="services"></param>
        public static void AddDnsClient(this IServiceCollection services)
        {
            services.AddSingleton<IDnsQuery>(b =>
            {
                var serviceOption = b.GetRequiredService<IOptions<ServiceDiscoveryOptions>>();
                if(serviceOption.Value ==null)
                {
                    return new LookupClient(IPAddress.Parse("127.0.0.1"), 8600);
                }
                //添加Consul服务地址
                return new LookupClient(serviceOption.Value.Consul.DnsEndpoint.ToIPEndPoint());
            });
        }

        /// <summary>
        /// 启用Consul 服务注册与发现
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        /// <param name="applicationLifetime"></param>
        /// <param name="consulClient"></param>
        /// <param name="serviceOptions"></param>
        public static IApplicationBuilder UseConsul(this IApplicationBuilder app)
        {
            #region 向Consul进行服务注册     

            var applicationLife = app.ApplicationServices.GetService<IApplicationLifetime>();
            if (applicationLife == null) throw new ArgumentNullException(nameof(applicationLife));

            var consulClient = app.ApplicationServices.GetService<IConsulClient>();
            if (consulClient == null) throw new ArgumentNullException(nameof(consulClient));

            var serviceOptions = app.ApplicationServices.GetRequiredService<IOptions<ServiceDiscoveryOptions>>();
            if (serviceOptions == null) throw new ArgumentNullException(nameof(serviceOptions));

            var env = app.ApplicationServices.GetRequiredService<IHostingEnvironment>();
            if (env == null) throw new ArgumentNullException(nameof(env));


            //获取服务启动地址绑定信息
            var features = app.Properties["server.Features"] as FeatureCollection;
            var addresses = features.Get<IServerAddressesFeature>()
                .Addresses
                .Select(p => new Uri(p));


            //在服务启动时,向Consul 中心进行注册
            applicationLife.ApplicationStarted.Register(() => {

                foreach (var address in addresses)
                {
                    //设定服务Id(全局唯一 unique）
                    var serviceId = $"{serviceOptions.Value.ServiceName}_{address.Host}:{address.Port}";

                    //设置健康检查方法
                    var httpCheck = new AgentServiceCheck()
                    {
                        DeregisterCriticalServiceAfter = TimeSpan.FromMinutes(1),  //错误时间超过1分钟，移除
                        Interval = TimeSpan.FromSeconds(30),                       //30秒检查一下
                        HTTP = new Uri(address, "HealthCheck").OriginalString
                    };
                    //设置Consul中心 配置
                    var registration = new AgentServiceRegistration()
                    {
                        Checks = new[] { httpCheck }, //配置健康检查
                        Address = address.Host,       //Consul 地址
                        Port = address.Port,          //Consul 端口
                        ID = serviceId,               //服务唯一ID
                        Name = serviceOptions.Value.ServiceName,   //对外服务名称

                    };
                    //向Consul 中心进行注册
                    consulClient.Agent.ServiceRegister(registration).GetAwaiter().GetResult();

                }
            });

            //在程序停止时,向Consul 中心进行注销
            applicationLife.ApplicationStopped.Register(() =>
            {
                foreach (var address in addresses)
                {
                    //设定服务Id(全局唯一 unique）
                    var serviceId = $"{serviceOptions.Value.ServiceName}_{address.Host}:{address.Port}";
                    consulClient.Agent.ServiceDeregister(serviceId).GetAwaiter().GetResult();
                }
            });

            return app;
            #endregion
        }
    }
}
