using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Data
{
    public static class ConstantData
    {
        public static string MongoDbConnectionString = "mongodb://dbadmin:IamAdmin%237841@15.206.42.43:27017";
        public static string MongoDbName = "nexat";
        public static string MongoDbCollection = "nexat-context";
        public static string MongoDbCollectionForQueueManager = "nexat-queue";
    }
}
