using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using StatusEngine.Demo.Model;

namespace StatusEngine.Demo
{
    public interface IUpdatesHub
    {
        Task AssetOwnerUpdate(OwnerSummary ownerSummary);
    }

    public class UpdatesHub : Hub<IUpdatesHub>
    {
        public Task Send(OwnerSummary ownerSummary)
        {
            return Clients.All.AssetOwnerUpdate(ownerSummary);
        }
    }
}
