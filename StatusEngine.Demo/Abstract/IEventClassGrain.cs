using System.Threading.Tasks;
using Orleans;
using StatusEngine.Demo.Model;

namespace StatusEngine.Demo.Abstract
{
    public interface IEventClassGrain : IGrainWithStringKey
    {
        Task<EventClassSummary> Get();
        Task Notify(Event @event);
    }
}