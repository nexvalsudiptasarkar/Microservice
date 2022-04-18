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
    public class EarlyCheckOutController : ControllerBase
    {
        private readonly IBus _bus;
        private readonly ILogger<EarlyCheckOutManager> _logger;
        private readonly IConfiguration _config;
        public EarlyCheckOutController(IBus bus, IConfiguration config)
        {
            _bus = bus;
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
                var x = db.LoadDocumentByQueueActionIdentifier<GlobalConfig>(_config.GetSection("QueueActionIdentifier")["earlyCheckOutQueue"]);
                queueObj = db.LoadDocumentByMessageQueueIdentifier<GlobalQueueConfig>("RabbitMq");
                if (x == null)
                {
                    globalObj.CurrentEnv = "UAT";
                    globalObj.QueueActionIdentifier = _config.GetSection("QueueActionIdentifier")["earlyCheckOutQueue"];
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


            if (lst != null && lst.Count() > default(int))
            {
                foreach (var obj in lst)
                {
                    SerializedJson json = new SerializedJson();
                    json.consumer = TargetConsumer.EarlyCheckOutConsumer;
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
