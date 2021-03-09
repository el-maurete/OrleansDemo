using StatusEngine.Demo.Model;

namespace StatusEngine.Demo.Events
{
    public class EventClassChangedStatus
    {
        public Event Event { get; set; }
        public EventClassSummary EventClassStatus { get; set; }
    }
}