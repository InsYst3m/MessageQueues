using AOP.DynamicProxy;
using AOP.Logger;
using Autofac;
using Autofac.Extras.DynamicProxy;
using System;

namespace MainServer
{
    public class Program
    {
        private static IContainer AutofacContainer()
        {
            var builder = new ContainerBuilder();

            builder.Register(b => new MethodInterceptor(new AopLogger(), new AopMapper()));

            builder.RegisterType<MessageQueueListener>()
                   .As<IMessageQueueListener>()
                   .EnableInterfaceInterceptors()
                   .InterceptedBy(typeof(MethodInterceptor));

            return builder.Build();
        }

        public static void Main(string[] args)
        {
            //var messageQueueListener = new MessageQueueListener();
            var messageQueueListener = AutofacContainer().Resolve<IMessageQueueListener>();
            messageQueueListener.Listen();
            messageQueueListener.TestMethodWithArgs("string arg 1", 15);

            Console.WriteLine("Press [enter] to exit.");
            Console.ReadLine();

            messageQueueListener.Dispose();
        }
    }
}
