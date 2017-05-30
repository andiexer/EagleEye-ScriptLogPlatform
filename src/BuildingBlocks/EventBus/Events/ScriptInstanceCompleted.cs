using System;
using System.Collections.Generic;
using System.Text;

namespace EESLP.BuilidingBlocks.EventBus.Events
{
    public class ScriptInstanceCompleted : IEvent
    {
        public int Id { get; set; }
        public string Status { get; set; }

        public ScriptInstanceCompleted(int id, string status)
        {
            Id = id;
            Status = status;
        }
    }
}
