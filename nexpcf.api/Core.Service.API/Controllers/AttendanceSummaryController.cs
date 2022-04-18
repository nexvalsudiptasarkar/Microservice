using Core.Producer.API.Business;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Shared.Data;
using Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Core.Service.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AttendanceSummaryController : ControllerBase
    {
        private readonly IBus _bus;
        private readonly ILogger<AttendanceSummaryManager> _logger;
        private readonly IConfiguration _config;
        public AttendanceSummaryController(IBus bus, ILogger<AttendanceSummaryManager> logger, IConfiguration config)
        {
            _bus = bus;
            _logger = logger;
            _config = config;
        }

        [HttpPost]
        public async Task<IActionResult> CreateRequest(IEnumerable<SummaryRef> lst)
        {
            GlobalConfig globalObj = new GlobalConfig();
            GlobalQueueConfig queueObj = new GlobalQueueConfig();
            try
            {
                MongoDbDataHelper db = new MongoDbDataHelper();
                var x = db.LoadDocumentByQueueActionIdentifier<GlobalConfig>(_config.GetSection("QueueActionIdentifier")["attendanceSummaryQueue"]);
                queueObj = db.LoadDocumentByMessageQueueIdentifier<GlobalQueueConfig>("RabbitMq");
                if (x == null)
                {
                    globalObj.CurrentEnv = "UAT";
                    globalObj.QueueActionIdentifier = _config.GetSection("QueueActionIdentifier")["attendanceSummaryQueue"];
                    globalObj.PCFEnabled = "1";
                    db.InsertDocument<GlobalConfig>(globalObj);
                }
                else
                {
                    globalObj.CurrentEnv = x.CurrentEnv;
                    globalObj.QueueActionIdentifier = x.QueueActionIdentifier;
                    globalObj.PCFEnabled = x.PCFEnabled;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("{0} :: {1} :: {2}", this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message));
            }

            int _param = default(int);
            int.TryParse(globalObj.PCFEnabled, out _param);
            if (_param == PCFEnabled.Disabled.GetHashCode())
            {
                await Task.Run(async () =>
                {
                    using (AttendanceSummaryManager objectModel = new AttendanceSummaryManager(_logger))
                    {
                        await objectModel.generateAttendanceSummaryAutoProcess();
                    }
                });

                return Ok(new { Success = true });
            }
            else 
            {
                if (lst != null && lst.Count() > default(int))
                {
                    foreach (var obj in lst)
                    {
                        SerializedJson json = new SerializedJson();
                        json.consumer = TargetConsumer.AttendanceSummaryConsumer;
                        json.JsonData = JsonConvert.SerializeObject(obj);

                        Uri uri = new Uri(string.Format("{0}/{1}", queueObj.MessageQueueUrl, globalObj.QueueActionIdentifier));
                        var endPoint = await _bus.GetSendEndpoint(uri);
                        await endPoint.Send(json);
                    }

                    return Ok(new PcfApiResponse() { Success = true, Message = "Job queued successfully", Data = null });
                }
                return BadRequest();
            }
        }
    }
}
