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
    public class AttendanceSummaryConsumer : IBaseConsumer
    {
        private readonly ILogger<BaseConsumer> _logger;

        public AttendanceSummaryConsumer(ILogger<BaseConsumer> logger)
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
                /*_logger.LogInformation(string.Format("{0} :: {1} :: {2} :: {3} CONSUMEAUTOPROCESS METHOD ENTERED",
                this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name,
                DateTime.Now, data));*/


                if (data.objectid > 0)
                {
                    using (AttendanceSummaryManagerSync objManager = new AttendanceSummaryManagerSync(_logger))
                    {
                        /*_logger.LogInformation(string.Format("{0} :: {1} :: {2} :: {3} MAIN PROCESS STARED",
                        this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name,
                        DateTime.Now, data));*/
                        await objManager.generateAttendanceSummaryAutoProcess();
                        /*_logger.LogInformation(string.Format("{0} :: {1} :: {2} :: {3} MAIN PROCESS ENDED",
                        this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name,
                        DateTime.Now, data));*/
                    }
                }
            }
            catch (Exception e)
            {
                _logger.LogCritical(string.Format("{0} :: {1} :: AttendanceSummaryConsumer {2}",
                           this.GetType().Name, "ConsumeAutoProcess",
                                e.Message));
            }
            /*_logger.LogInformation(string.Format("{0} :: {1} :: {2} :: {3} CONSUMEAUTOPROCESS METHOD EXISTED",
            this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name,
            DateTime.Now, data));*/

        }
    }
}

