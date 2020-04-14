using System;

namespace Shared.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class EndPointDataAttribute : Attribute
    {
        public string Code;
        public EndPointDataAttribute(string code)
        {
            Code = code;
        }
    }
}
