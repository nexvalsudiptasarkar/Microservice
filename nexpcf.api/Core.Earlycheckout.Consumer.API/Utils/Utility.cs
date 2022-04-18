using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Consumer.API.Utils
{
    public static class Utility
    {
        public static T Deserialize<T>(string json)
        {
            T objectResult = JsonConvert.DeserializeObject<T>(json);
            return objectResult;
        }
    }
}
