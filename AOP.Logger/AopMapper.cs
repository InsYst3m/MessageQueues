using System;

namespace AOP.Logger
{
    [Serializable]
    public class AopMapper : IAopMapper
    {
        public MethodLogModel Map(string methodName, string message, string arguments, string dateTime)
        {
            return new MethodLogModel
            {
                Name = methodName,
                Message = message,
                Arguments = arguments,
                DateTime = dateTime
            };
        }
    }
}
