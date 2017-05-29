using System;
using System.Collections.Generic;
using System.Text;

namespace EESLP.BuilidingBlocks.EventBus.Events
{
    public class ScriptInstanceCreated : IEvent
    {
        public int ScriptInstanceId { get; set; }
        public string Description  { get; set; }

        protected ScriptInstanceCreated()
        {
        }

        public ScriptInstanceCreated(int id, string description)
        {
            ScriptInstanceId = id;
            Description = description;
        }


    }
}
