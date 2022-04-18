using System;

namespace Shared.Data
{
    [AttributeUsage(AttributeTargets.Property)]
    public class outputparam : Attribute
    {
        public ParamType ParamType { get; set; }
    }

    public enum ParamType
    {
        Input = 0,
        Output = 1,
        InputOutput = 2
    }
}
