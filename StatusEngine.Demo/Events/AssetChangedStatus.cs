using StatusEngine.Demo.Model;

namespace StatusEngine.Demo.Events
{
    public class AssetChangedStatus
    {
        public AssetSummary Asset { get; set; }
    }
}