namespace StatusEngine.Demo.Model
{
    public class Event
    {
        public string Owner { get; set; }
        public string AssetClassName { get; set; }
        public string AssetName { get; set; }
        public string EventClassName { get; set; }
        public string EventId { get; set; }
        public string Summary { get; set; }
        public int Data { get; set; }
    }
}
