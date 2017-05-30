using System;
using System.Collections.Generic;
using System.Text;

namespace EESLP.BuilidingBlocks.EventBus.Events
{
    public class ScriptInstanceCreated : IEvent
    {
        public int Id { get; set; }

        public ScriptInstanceCreated(int id)
        {
            Id = id;
        }
    }
}
