using StatusEngine.Demo.Events;
using StatusEngine.Demo.Model;

namespace StatusEngine.Demo.States
{
    public class EventClass
    {
        public Colour Colour { get; set; } 
        public Event LatestEvent { get; set; }

        public EventClass Apply(EventClassChangedStatus @event)
        {
            LatestEvent = @event.Event;
            Colour = @event.EventClassStatus.Colour;
            return this;
        }
    }
}
