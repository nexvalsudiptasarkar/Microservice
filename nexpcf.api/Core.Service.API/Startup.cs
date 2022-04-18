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
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Core.Service.API
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
            GlobalQueueConfig obj = new GlobalQueueConfig();
            try
            {
                MongoDbDataHelper db = new MongoDbDataHelper();
                var x = db.LoadDocumentByMessageQueueIdentifier<GlobalQueueConfig>("RabbitMq");
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
                x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(config =>
                {
                    config.UseHealthCheck(provider);
                    config.Host(obj.MessageQueueUrl, h =>
                    {
                        h.Username(obj.MessageQueueUsername);
                        h.Password(obj.MessageQueuePassword);
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
            loggerFactory.AddFile($"{path}\\Logs\\Producer\\Log.txt");*/
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
