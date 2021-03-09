using System.Threading.Tasks;
using Orleans;
using StatusEngine.Demo.Events;
using StatusEngine.Demo.Model;

namespace StatusEngine.Demo.Abstract
{
    public interface IAssetGrain : IGrainWithStringKey
    {
        Task<AssetSummary> Get();
        Task Notify(EventClassChangedStatus @event);
    }
}
