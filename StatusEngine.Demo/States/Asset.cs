using System.Collections.Generic;
using StatusEngine.Demo.Events;
using StatusEngine.Demo.Model;

namespace StatusEngine.Demo.States
{
    public class Asset
    {
        public Dictionary<string, EventClassSummary> EventClasses { get; set; }
            = new Dictionary<string, EventClassSummary>();
        
        public Asset Apply(EventClassChangedStatus @event)
        {
            EventClasses[@event.EventClassStatus.Name] = @event.EventClassStatus;
            return this;
        }
    }
}
