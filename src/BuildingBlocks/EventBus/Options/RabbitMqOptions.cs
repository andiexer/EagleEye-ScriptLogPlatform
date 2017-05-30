using System;
using System.Collections.Generic;
using System.Text;
using RawRabbit.Configuration;

namespace EESLP.BuilidingBlocks.EventBus.Options
{
    public class RabbitMqOptions : RawRabbitConfiguration
    {
        public string Hostname { get; set; }
    }
}
