using AOP.Logger;
using PostSharp.Aspects;
using System;

namespace AOP.CodeRewriting
{
    [Serializable]
    public class LoggerAspectAttribute : OnMethodBoundaryAspect
    {
        private readonly IAopLogger _logger;
        private readonly IAopMapper _mapper;

        public LoggerAspectAttribute()
        {
            _logger = new AopLogger();
            _mapper = new AopMapper();
        }

        public override void OnEntry(MethodExecutionArgs args)
        {
            base.OnEntry(args);

            var methodName = args.Method.Name;
            var arguments = string.Join("|", args.Arguments.ToArray());
            var beforExecuteMessage = $"Execution of method: '{methodName}' was started. Arguments: '{arguments}'.";
            var beforeExecuteMethodLogModel = _mapper.Map(methodName, beforExecuteMessage, arguments, DateTime.Now.ToString("ddMMyyyyhhmmss"));

            _logger.LogMethod(beforeExecuteMethodLogModel);
        }

        public override void OnSuccess(MethodExecutionArgs args)
        {
            base.OnSuccess(args);

            var methodName = args.Method.Name;
            var arguments = string.Join("|", args.Arguments.ToArray());
            var afterExecuteMessage = $"Execution of method: '{methodName}' was completed. Arguments: '{arguments}'. Returned value: '{args.ReturnValue}'.";
            var afterExecuteMethodLogModel = _mapper.Map(methodName, afterExecuteMessage, arguments, DateTime.Now.ToString("ddMMyyyyhhmmss"));

            _logger.LogMethod(afterExecuteMethodLogModel);
        }

        public override void OnExit(MethodExecutionArgs args)
        {
            Console.WriteLine("Goodbye.");
        }
    }
}
