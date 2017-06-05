using System;
using System.Collections.Generic;
using System.Text;

namespace EESLP.BuilidingBlocks.EventBus.Events
{
    public class HostDeleted : IEvent
    {
        public int Id { get; set; }
        public HostDeleted(int id)
        {
            Id = id;
        }
    }
}
