using System;
using EgorLin.Collections.Unsafe;
using EgorLin.EventBus.Containers;
using EgorLin.EventBus.Keys;
using EgorLin.EventBus.Subscriptions;

namespace EgorLin.EventBus.Bus
{
	public class EventBus : IEventBusListener, IEventBusPublisher
	{
        private readonly IntHashMap<IEventContainer> _map = new();

        public IEventSubscription Subscribe<T>(KeyEvent<T> key, Action<T> callback)
        {
            var container = GetOrCreate<T>(key);
            container.Add(callback);
            return EventSubscription<T>.Create(container, callback);
        }

        public IEventSubscription Subscribe(KeyEvent key, Action callback)
        {
            var container = GetOrCreate(key);
            container.Add(callback);
            return EventSubscription.Create(container, callback);
        }

        public void Publish<T>(KeyEvent<T> key, T value)
        {
            if (_map.TryGetValue(key.Id, out var raw))
            {
                ((EventContainer<T>)raw!).Invoke(value);
            }
        }
        
        public void Publish(KeyEvent key)
        {
            if (_map.TryGetValue(key.Id, out var raw))
            {
                (((EventContainer)raw)!).Invoke();
            }
        }
        
        private EventContainer<T> GetOrCreate<T>(KeyEvent<T> key)
        {
            if (_map.TryGetValue(key.Id, out var raw))
            {
                return (EventContainer<T>)raw;
            }

            var container = new EventContainer<T>();
            _map.Set(key.Id, container);
            
            return container;
        }
        
        private EventContainer GetOrCreate(KeyEvent key)
        {
            if (_map.TryGetValue(key.Id, out var raw))
            {
                return (EventContainer)raw;
            }

            var container = new EventContainer();
            _map.Set(key.Id, container);
            
            return container;
        }
	}
}