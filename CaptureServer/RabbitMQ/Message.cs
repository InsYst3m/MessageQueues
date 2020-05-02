﻿using System;

namespace CaptureServer.RabbitMQ
{
    [Serializable]
    public class Message
    {
        public string FileName { get; set; }
        public byte[] Content { get; set; }
    }
}
