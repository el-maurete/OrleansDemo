using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Orleans;
using StatusEngine.Demo.Abstract;
using StatusEngine.Demo.Model;

namespace StatusEngine.Demo
{
    public class DemoClient : IHostedService
    {
        private readonly ILogger<DemoClient> _logger;
        private readonly IClusterClient _client;
        private bool _run;
        private Task _task;

        public DemoClient(ILogger<DemoClient> logger, IClusterClient client)
        {
            _logger = logger;
            _client = client;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _run = true;
            _task = Loop();
            await Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _run = false;
            await _task;
        }

        private async Task Loop()
        {
            while (_run)
            {
                await Task.Delay(100);
                _logger.LogInformation("Demo loop activated");
                try
                {
                    var demo = GenerateDemoEvent();
                    var grain = _client.GetGrain<IAssetGrain>(demo.AssetGrainPath());
                    var assetBefore = await grain.Get();

                    var eventClassGrain = _client.GetGrain<IEventClassGrain>(demo.EventClassGrainPath());
                    await eventClassGrain.Notify(demo);

                    var assetAfter = await grain.Get();
                    
                    _logger.LogInformation("{Asset} - BEFORE : {Colour}", demo.AssetGrainPath(), assetBefore.Colour);
                    _logger.LogInformation("{Asset} - AFTER  : {Colour}", demo.AssetGrainPath(), assetAfter.Colour);
                }
                catch (Exception exc)
                {
                    _logger.LogError(exc, "Demo loop errored");
                }
                _logger.LogInformation("Demo loop back to sleep");
            }
        }

        private Event GenerateDemoEvent()
        {
            var random = new Random(DateTime.Now.Millisecond);
            var owner = "TEAM-" + random.Next(6);
            var assetClassName = "AssetClass-" + random.Next(10);
            var assetName = "Asset-" + random.Next(5);
            var eventClassName = "EventClass-" + random.Next(5);
            return new Event
            {
                Data = 1 - random.Next(8),
                Summary = "Demo event @ " + DateTime.Now,
                Owner = owner,
                AssetClassName = assetClassName,
                AssetName = assetName,
                EventClassName = eventClassName,
                EventId = Guid.NewGuid().ToString()
            };
        }
    }
}
