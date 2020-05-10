using Castle.DynamicProxy;
using AOP.Logger;
using System;

namespace AOP.DynamicProxy
{
    public class MethodInterceptor : IInterceptor
    {
        private readonly IAopLogger _logger;
        private readonly IAopMapper _mapper;

        public MethodInterceptor(IAopLogger logger, IAopMapper mapper)
        {
            _logger = logger;
            _mapper = mapper;
        }

        public void Intercept(IInvocation invocation)
        {
            var methodName = invocation.Method.Name;
            var args = string.Join("|", invocation.Arguments);
            var beforExecuteMessage = $"Execution of method: '{methodName}' was started. Arguments: '{args}'.";
            var beforeExecuteMethodLogModel = _mapper.Map(methodName, beforExecuteMessage, args, DateTime.Now.ToString("ddMMyyyyhhmmss"));

            _logger.LogMethod(beforeExecuteMethodLogModel);

            invocation.Proceed();

            var afterExecuteMessage = $"Execution of method: '{methodName}' was completed. Arguments: '{args}'. Returned value: '{invocation.ReturnValue}'.";
            var afterExecuteMethodLogModel = _mapper.Map(methodName, afterExecuteMessage, args, DateTime.Now.ToString("ddMMyyyyhhmmss"));
            _logger.LogMethod(afterExecuteMethodLogModel);
        }
    }
}
