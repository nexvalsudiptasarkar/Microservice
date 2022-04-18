using System;

namespace Shared.Models
{
    public class SerializedJson
    {
        public TargetConsumer consumer { get; set; }
        public string JsonData { get; set; }
    }

    public class ModelBase
    {
        protected DateTime? _CreatedOn = null;
        protected DateTime? _ModifiedOn = null;
    }

    public class PcfApiResponse
    {
        public bool Success { get; set; }
        public object Data { get; set; }
        public string Message { get; set; }
    }

    public enum TargetConsumer
    {
        AttendanceSummaryConsumer = 1,
        EarlyCheckOutConsumer = 2
    }

    public enum PCFEnabled
    { 
        Enabled = 1,
        Disabled = 0
    }
}
