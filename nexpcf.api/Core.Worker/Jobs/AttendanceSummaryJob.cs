using Core.Worker.Business;
using Core.Worker.Utility_Classes;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using RestSharp;
using Shared.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Worker.Jobs
{
    public class AttendanceSummaryJob
    {
        private readonly ILogger<Worker> _logger;
        private readonly IConfiguration _configuration;

        public AttendanceSummaryJob(ILogger<Worker> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public async Task generateAttendanceSummaryAsync()
        {
            try
            {
                /*using (AttendanceSummaryManager objManager = new AttendanceSummaryManager())
                {
                    //await objManager.generateAttendanceSummaryAutoProcess();
                }*/

                //using (AttendanceSummaryManager objectModel = new AttendanceSummaryManager())
                //{
                //    await objectModel.generateAttendanceSummaryAutoProcess();
                //}

                Console.WriteLine("Task Running");
                
                /*List<SummaryRef> lst = new List<SummaryRef>();
                lst.Add(new SummaryRef()
                {
                    objectid = 1,
                    moduleid = 1
                });
                lst.Add(new SummaryRef()
                {
                    objectid = 2,
                    moduleid = 1
                });
                var request = new RestRequest().AddJsonBody(lst);
                IConfiguration config = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: false, reloadOnChange: false).Build();
                var response = await new PCFRestClient(config).ExecuteAsync(request, "AttendanceSummary/CreateRequest", Method.Post);*/
            }
            catch (Exception e)
            {
                _logger.LogInformation("updated daemon running at: {time} ", DateTimeOffset.Now);
                throw e;
            }
        }

    }
}
