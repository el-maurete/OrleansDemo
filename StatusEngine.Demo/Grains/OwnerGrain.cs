using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Orleans;
using StatusEngine.Demo.Abstract;
using StatusEngine.Demo.Events;
using StatusEngine.Demo.Model;
using StatusEngine.Demo.States;

namespace StatusEngine.Demo.Grains
{
    public class OwnerGrain : BaseGrain<Owner>, IOwnerGrain
    {
        private readonly ILogger<OwnerGrain> _logger;
        private readonly IHubContext<UpdatesHub, IUpdatesHub> _hub; 
        
        public OwnerGrain(ILogger<OwnerGrain> logger, IHubContext<UpdatesHub, IUpdatesHub> hub)
        {
            _logger = logger;
            _hub = hub;
        }
        
        public Task<OwnerSummary> Get()
        {
            var colour = State.Assets
                .Select(x => x.Value.Colour)
                .DefaultIfEmpty(Colour.unknown)
                .Max();
            
            return Task.FromResult(new OwnerSummary
            {
                Name = this.GetPrimaryKeyString().Name(),
                Colour = colour,
                Reason = GetReason()
            });
        }

        public async Task Notify(AssetChangedStatus @event)
        {
            var currentStatus = await Get();
            Log("Received notification: Asset colour has changed!");
            RaiseEvent(@event);
            await ConfirmEvents();
            var newStatus = await Get();
            Warn($"Owner color is {newStatus.Colour}");
            await _hub.Clients.All.AssetOwnerUpdate(new OwnerSummary
            {
                Name = this.GetPrimaryKeyString(),
                Colour = newStatus.Colour,
                Reason = GetReason()
            });
        }

        private string GetReason()
        {
            return string.Join(", ", State.Assets
                .ToLookup(x => x.Value.Colour)
                .OrderBy(x => x.Key)
                .Select(x => x.Key + ": " + x.Count()));
        }

        private void Log(string message)
            => _logger.LogInformation("{Key}: {Message}", this.GetPrimaryKeyString().Name(), message);
        
        private void Warn(string message)
            => _logger.LogWarning("{Key}: {Message}", this.GetPrimaryKeyString().Name(), message);
    }
}
