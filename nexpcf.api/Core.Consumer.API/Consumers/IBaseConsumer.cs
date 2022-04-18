using MassTransit;
using Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Consumer.API.Consumers
{
    public interface IBaseConsumer
    {
        public Task ConsumeAutoProcess(ConsumeContext<SerializedJson> json);
    }
}
