using MongoDB.Bson;
using Newtonsoft.Json.Bson;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Models
{
    public class GlobalConfig
    {
        public ObjectId Id { get; set; }
        public string PCFEnabled { get; set; }
        public string CurrentEnv { get; set; }
        public string QueueActionIdentifier { get; set; }
    }

    public class GlobalQueueConfig
    {
        public ObjectId Id { get; set; }
        public string MessageQueueIdentifier { get; set; }
        public string MessageQueueUrl { get; set; }
        public string MessageQueueUsername { get; set; }
        public string MessageQueuePassword { get; set; }
    }
}
