using Core.Consumer.API.Business;
using Core.Consumer.API.Utils;
using Core.Earlycheckout.Consumer.API.Consumers;
using MassTransit;
using Microsoft.Extensions.Logging;
using Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Earlycheckout.Consumer.API.Consumers
{
    public class EarlyCheckOutConsumer : IBaseConsumer
    {
        private readonly ILogger<BaseConsumer> _logger;

        public EarlyCheckOutConsumer(ILogger<BaseConsumer> logger)
        {
            _logger = logger;
        }
        public async Task ConsumeAutoProcess(ConsumeContext<SerializedJson> json)
        {
            /*_logger.LogInformation("Info log");
            _logger.LogWarning("Warning log");
            _logger.LogError("Error log");
            _logger.LogDebug("Debug log");
            _logger.LogCritical("Critical log");
            _logger.LogTrace("Trace log");*/

            SummaryRef data = Utility.Deserialize<SummaryRef>(json.Message.JsonData);

            if (data.objectid > 0)
            {
                using (EarlyCheckOutManager objManager = new EarlyCheckOutManager(_logger))
                {
                    await objManager.processEarlyCheckOut();
                }
            }
            //ThreadPool.QueueUserWorkItem(t =>
            //{
            //  Validate the Data
            //  Store/Fetch to/from Database
            //  Notify the user via Email / SMS
            //});
        }
    }
}

