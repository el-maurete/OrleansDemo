using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Orleans;
using StatusEngine.Demo.Abstract;

namespace StatusEngine.Demo.Controllers
{
    [Route("[controller]")]
    public class AssetOwnersController : ControllerBase
    {
        private readonly ILogger<AssetOwnersController> _logger;
        private readonly IClusterClient _client;

        public AssetOwnersController(ILogger<AssetOwnersController> logger, IClusterClient client)
        {
            _logger = logger;
            _client = client;
        }

        [HttpGet("")]
        public async Task<IActionResult> Get()
        {
            var owners = 
                from file in Directory.GetFiles("bin", "*.log")
                let ownerName = file.Substring(4, file.Length - 8)
                let grain = _client.GetGrain<IOwnerGrain>(ownerName)
                select grain.Get();

            return Ok(await Task.WhenAll(owners));
        }
    }
}