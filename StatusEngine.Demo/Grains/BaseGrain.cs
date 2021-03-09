using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Orleans;
using Orleans.EventSourcing;
using Orleans.EventSourcing.CustomStorage;
using Orleans.Providers;

namespace StatusEngine.Demo.Grains
{
    [LogConsistencyProvider(ProviderName = "Default")]
    [StorageProvider(ProviderName = "Default")]
    public abstract class BaseGrain<TState> :
        JournaledGrain<TState, object>,
        ICustomStorageInterface<TState, object>
        where TState : class, new()
    {
        private string FileName => $"bin/{this.GetPrimaryKeyString()}.log";

        async Task<bool> ICustomStorageInterface<TState, object>.ApplyUpdatesToStorage(IReadOnlyList<object> updates, int expectedVersion)
        {
            if (Version != expectedVersion)
                return false;

            var dirName = Path.GetDirectoryName(FileName);
            if (!string.IsNullOrEmpty(dirName) && !Directory.Exists(dirName))
                Directory.CreateDirectory(dirName);
            
            await File.AppendAllLinesAsync(FileName, updates
                .Select(raisedEvent => JsonConvert.SerializeObject(new StoredEvent
                {
                    Event = JsonConvert.SerializeObject(raisedEvent),
                    EventType = raisedEvent.GetType().FullName
                })));

            return true;
        }

        async Task<KeyValuePair<int, TState>> ICustomStorageInterface<TState, object>.ReadStateFromStorage()
        {
            var events = File.Exists(FileName)
                ? ( from json in await File.ReadAllLinesAsync(FileName)
                    let storedEvent = JsonConvert.DeserializeObject<StoredEvent>(json)
                    let actualEvent = JsonConvert.DeserializeObject(storedEvent.Event, Type.GetType(storedEvent.EventType))
                    select actualEvent)
                    .ToList()
                : new List<object>();

            var state = events.Aggregate(State, Apply);
            
            return new KeyValuePair<int, TState>(events.Count, state);
        }

        private static TState Apply(TState aggregatedState, object eventData) =>
            ((dynamic) aggregatedState).Apply((dynamic) eventData);
        
        public class StoredEvent
        {
            public string Event { get; set; }
            public string EventType { get; set; }
        }
    }
}
