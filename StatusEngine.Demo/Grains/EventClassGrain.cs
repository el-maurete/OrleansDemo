using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Orleans;
using StatusEngine.Demo.Abstract;
using StatusEngine.Demo.Events;
using StatusEngine.Demo.Model;
using StatusEngine.Demo.Services;
using StatusEngine.Demo.States;

namespace StatusEngine.Demo.Grains
{
    public class EventClassGrain : BaseGrain<EventClass>, IEventClassGrain
    {
        private readonly ILogger<EventClassGrain> _logger;
        private readonly IClusterClient _client;
        private readonly IStatusEngine _statusEngine;

        public EventClassGrain(
            ILogger<EventClassGrain> logger,
            IClusterClient client,
            IStatusEngine statusEngine)
        {
            _logger = logger;
            _client = client;
            _statusEngine = statusEngine;
        }

        public Task<EventClassSummary> Get()
        {
            return Task.FromResult(new EventClassSummary
            {
                Name = this.GetPrimaryKeyString().Name(),
                Colour = State.Colour
            });
        }

        public async Task Notify(Event @event)
        {
            Log("Received a new event");
            var result = _statusEngine.Calculate(State.Colour, @event.Data);
            if (result == State.Colour)
            {
                Log("Nothing has changed. Ignoring");
                return;
            }

            Log($"Event class colour has changed to {result}!");
            var newEvent = new EventClassChangedStatus
            {
                Event = @event,
                EventClassStatus = new EventClassSummary
                {
                    Name = this.GetPrimaryKeyString().Name(),
                    Colour = result
                }
            };
            
            RaiseEvent(newEvent);
            var parentPath = this.GetPrimaryKeyString().ParentPath();
            var assetGrain = _client.GetGrain<IAssetGrain>(parentPath);
            await assetGrain.Notify(newEvent);
            Log("Asset notified");
            await ConfirmEvents();
            Log("Done");
        }

        private void Log(string message)
            => _logger.LogInformation("{Key}: {Message}", this.GetPrimaryKeyString().Name(), message);
    }
}
