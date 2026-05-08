using System;
using EgorLin.EventBus.Containers;
using EgorLin.Pools;

namespace EgorLin.EventBus.Subscriptions
{
    public class EventSubscription<T> : IEventSubscription
    {
        private EventContainer<T> Container;
        private Action<T> Callback;

        internal static EventSubscription<T> Create(EventContainer<T> container, Action<T> callback)
        {
            var subscription = PoolClass<EventSubscription<T>>.Spawn();
            
            subscription.Container = container;
            subscription.Callback = callback;

            return subscription;
        }

        public void Release()
        {
            Container.Remove(Callback);
            
            Container = null;
            Callback = null;
            
            PoolClass<EventSubscription<T>>.Recycle(this);
        }
    }
    
    public class EventSubscription : IEventSubscription
    {
        private EventContainer Container;
        private Action Callback;

        internal static EventSubscription Create(EventContainer container, Action callback)
        {
            var subscription = PoolClass<EventSubscription>.Spawn();
            
            subscription.Container = container;
            subscription.Callback = callback;

            return subscription;
        }

        public void Release()
        {
            Container.Remove(Callback);
            
            Container = null;
            Callback = null;
            
            PoolClass<EventSubscription>.Recycle(this);
        }
    }
}