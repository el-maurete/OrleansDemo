using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Orleans;
using StatusEngine.Demo.Abstract;
using StatusEngine.Demo.Events;
using StatusEngine.Demo.Model;
using StatusEngine.Demo.States;

namespace StatusEngine.Demo.Grains
{
    public class AssetGrain : BaseGrain<Asset>, IAssetGrain
    {
        private readonly ILogger<AssetGrain> _logger;
        private readonly IClusterClient _client;

        public AssetGrain(ILogger<AssetGrain> logger, IClusterClient client)
        {
            _logger = logger;
            _client = client;
        }

        public Task<AssetSummary> Get()
        {
            return Task.FromResult(GetSummary(State));
        }
        
        public async Task Notify(EventClassChangedStatus @event)
        {
            Log("Received notification: Event class colour has changed!");

            var currentStatus = GetSummary(State);
            RaiseEvent(@event);
            var newStatus = GetSummary(TentativeState);

            if (currentStatus.Colour == newStatus.Colour)
            {
                Log("Asset state did not change");
            }
            else
            {
                Log($"Asset colour has changed to {newStatus.Colour}!");
                var parentPath = this.GetPrimaryKeyString().ParentPath();
                var assetClassGrain = _client.GetGrain<IOwnerGrain>(parentPath);
                await assetClassGrain.Notify(new AssetChangedStatus
                {
                    Asset = newStatus
                });
                Log("Owner notified");
            }

            await ConfirmEvents();
        }
        
        private AssetSummary GetSummary(Asset state)
        {
            return new AssetSummary
            {
                Name = this.GetPrimaryKeyString().Name(),
                Colour = state.EventClasses?
                    .Select(x => x.Value.Colour)
                    .DefaultIfEmpty(Colour.unknown)
                    .Max() ?? Colour.unknown
            };
        }

        private void Log(string message)
            => _logger.LogInformation("{Key}: {Message}", this.GetPrimaryKeyString().Name(), message);
    }
}
