using System.Collections.Generic;
using StatusEngine.Demo.Events;
using StatusEngine.Demo.Model;

namespace StatusEngine.Demo.States
{
    public class Owner
    {
        public Dictionary<string, AssetSummary> Assets { get; set; }
            = new Dictionary<string, AssetSummary>();

        public Owner Apply(AssetChangedStatus @event)
        {
            Assets[@event.Asset.Name] = @event.Asset;
            return this;
        }
    }
}