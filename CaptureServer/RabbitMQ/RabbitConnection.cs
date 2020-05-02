﻿using RabbitMQ.Client;

namespace CaptureServer.RabbitMQ
{
    public class RabbitConnection
    {
        // TODO: configuration json file for rabbitMQ settings (UserName + Password; host)
        public IConnection GetRabbitConnection()
        {
            ConnectionFactory factory = new ConnectionFactory
            {
                UserName = "guest",
                Password = "guest",
                VirtualHost = "/",
                HostName = "localhost"
            };

            IConnection connection = factory.CreateConnection();

            return connection;
        }
    }
}
