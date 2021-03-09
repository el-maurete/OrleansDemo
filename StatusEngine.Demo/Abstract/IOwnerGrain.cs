using System.Threading.Tasks;
using Orleans;
using StatusEngine.Demo.Events;
using StatusEngine.Demo.Model;

namespace StatusEngine.Demo.Abstract
{
    public interface IOwnerGrain : IGrainWithStringKey
    {
        Task<OwnerSummary> Get();
        Task Notify(AssetChangedStatus @event);
    }
}