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
    public class BaseConsumer : IConsumer<SerializedJson>
    {
        private readonly ILogger<BaseConsumer> _logger;

        public BaseConsumer(ILogger<BaseConsumer> logger)
        {
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<SerializedJson> context)
        {
            try
            {
                /*_logger.LogInformation(string.Format("{0} :: {1} :: {2} :: {3} CONSUME METHOD ENTERED",
                this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name,
                DateTime.Now, context));*/
                switch (context.Message.consumer)
                {
                    case TargetConsumer.AttendanceSummaryConsumer:
                        /*_logger.LogInformation(string.Format("{0} :: {1} :: {2} :: {3}  CASE 1",
                        this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name,
                        DateTime.Now, context));*/
                        await new AttendanceSummaryConsumer(_logger).ConsumeAutoProcess(context);
                        break;
                    case TargetConsumer.EarlyCheckOutConsumer:
                        /*_logger.LogInformation(string.Format("{0} :: {1} :: {2} :: {3} CASE 2",
                        this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name,
                        DateTime.Now, context));*/
                        await new EarlyCheckOutConsumer(_logger).ConsumeAutoProcess(context);
                        break;
                    default:
                        break;
                }
                /*_logger.LogInformation(string.Format("{0} :: {1} :: {2}  CONSUME METHOD EXITED",
                this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name,
                DateTime.Now));*/
            }
            catch (Exception e)
            {
                _logger.LogCritical(string.Format("{0} :: {1} :: BaseConsumer {2}",
                        this.GetType().Name, "Consume",
                             e.Message));
            }
        }
    }
}
