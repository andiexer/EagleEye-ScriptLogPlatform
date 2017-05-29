using System;
using System.Collections.Generic;
using System.Text;

namespace EESLP.BuilidingBlocks.EventBus.Events
{
    public class ScriptDeleted : IEvent
    {
        public int Id { get; set; }

        public ScriptDeleted(int id)
        {
            Id = id;
        }
    }
}
