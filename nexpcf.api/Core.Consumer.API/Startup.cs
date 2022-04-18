using Core.Consumer.API.Consumers;
using GreenPipes;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Shared.Data;
using Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Core.Consumer.API
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
            GlobalQueueConfig obj = new GlobalQueueConfig();
            GlobalConfig queueIdentityObj = new GlobalConfig();
            try
            {
                MongoDbDataHelper db = new MongoDbDataHelper();
                var x = db.LoadDocumentByMessageQueueIdentifier<GlobalQueueConfig>("RabbitMq");
                queueIdentityObj = db.LoadDocumentByQueueActionIdentifier<GlobalConfig>(Configuration.GetSection("AppInfo")["QueueActionIdentifier"]);
                if (x == null)
                {
                    obj.MessageQueueUrl = "amqps://b-d22620e1-e0ab-4f8c-80fd-6e66a82efb6e.mq.ap-south-1.amazonaws.com:5671"; ;
                    obj.MessageQueueIdentifier = "RabbitMq";
                    obj.MessageQueueUsername = "admin";
                    obj.MessageQueuePassword = "AdminNex#7841";
                    db.InsertDocument<GlobalQueueConfig>(obj, ConstantData.MongoDbCollectionForQueueManager);
                }
                else
                {
                    obj.MessageQueueUrl = x.MessageQueueUrl;
                    obj.MessageQueueIdentifier = x.MessageQueueIdentifier;
                    obj.MessageQueueUsername = x.MessageQueueUsername;
                    obj.MessageQueuePassword = x.MessageQueuePassword;
                }
            }
            catch (Exception ex)
            {

            }

            services.AddMassTransit(x =>
            {
                x.AddConsumer<BaseConsumer>();
                x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(cfg =>
                {
                    cfg.UseHealthCheck(provider);
                    cfg.Host(obj.MessageQueueUrl, h =>
                    {
                        h.Username(obj.MessageQueueUsername);
                        h.Password(obj.MessageQueuePassword);
                    });
                    cfg.ReceiveEndpoint(queueIdentityObj.QueueActionIdentifier, ep =>
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
