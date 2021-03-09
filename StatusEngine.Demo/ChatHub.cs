using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using StatusEngine.Demo.Model;

namespace StatusEngine.Demo
{
    public interface IChatHub
    {
        Task AssetOwnerUpdate(OwnerSummary ownerSummary);
    }

    public class ChatHub : Hub<IChatHub>
    {
        public Task Send(OwnerSummary ownerSummary)
        {
            return Clients.All.AssetOwnerUpdate(ownerSummary);
        }
    }
}
