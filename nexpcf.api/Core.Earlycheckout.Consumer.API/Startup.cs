using Core.Earlycheckout.Consumer.API.Consumers;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Core.Earlycheckout.Consumer.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        /*public void ConfigureServices(IServiceCollection services)
        {
            services.AddMassTransit(x =>
            {
                x.AddConsumer<BaseConsumer>();
                x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(cfg =>
                {
                    cfg.UseHealthCheck(provider);
                    cfg.Host("rabbitmq://localhost", h =>
                    {
                        h.Username("guest");
                        h.Password("guest");
                    });
                    cfg.ReceiveEndpoint("attendanceSummaryQueue", ep =>
                    {
                        ep.PrefetchCount = 5;
                        //ep.UseMessageRetry(r => r.Interval(2, 100));
                        ep.ConfigureConsumer<BaseConsumer>(provider);
                    });
                }));
            });
            services.AddMassTransitHostedService();
            services.AddControllers();
        }*/

        public void ConfigureServices(IServiceCollection services)
        {
            string actionname = Configuration["QueueActionIdentifier"];
            services.AddMassTransit(x =>
            {
                x.AddConsumer<BaseConsumer>();
                x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(cfg =>
                {
                    cfg.UseHealthCheck(provider);
                    cfg.Host("rabbitmq://localhost", h =>
                    {
                        h.Username("guest");
                        h.Password("guest");
                    });
                    cfg.ReceiveEndpoint(actionname, ep =>
                    {
                        ep.PrefetchCount = 0;
                        //ep.UseMessageRetry(r => r.Interval(2, 100));
                        ep.ConfigureConsumer<BaseConsumer>(provider);
                    });
                }));
            });
            services.AddMassTransitHostedService();
            services.AddControllers();

        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            /*var path = Directory.GetCurrentDirectory();
            loggerFactory.AddFile($"{path}\\Logs\\Consumer\\Log.txt");*/
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                loggerFactory.AddFile(string.Format("{0}{1}", Configuration.GetSection("LogPathInfo")["WindowsLogFilePath"], "Log.txt"));
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                loggerFactory.AddFile(string.Format("{0}{1}", Configuration.GetSection("LogPathInfo")["LinuxLogFilePath"], "Log.txt"));
            }

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
