using System;

namespace AOP.Logger
{
    public interface IAopMapper
    {
        MethodLogModel Map(string methodName, string message, string arguments, string dateTime);
    }
}
