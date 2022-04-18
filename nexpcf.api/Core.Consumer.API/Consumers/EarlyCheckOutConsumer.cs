using Core.Consumer.API.Business;
using Core.Consumer.API.Utils;
using MassTransit;
using Microsoft.Extensions.Logging;
using Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Consumer.API.Consumers
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
            try
            {
                SummaryRef data = Utility.Deserialize<SummaryRef>(json.Message.JsonData);

                if (data.objectid > 0)
                {
                    using (EarlyCheckOutManager objManager = new EarlyCheckOutManager(_logger))
                    {
                        await objManager.processEarlyCheckOut();
                    }
                }
            }
            catch(Exception e)
            {
                _logger.LogCritical(string.Format("{0} :: {1} :: EarlyCheckOutConsumer {2}",
                        this.GetType().Name, "ConsumeAutoProcess",
                             e.Message));
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

