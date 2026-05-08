
using EgorLin.Pools;

namespace EgorLin.EventBus.Subscriptions
{
	public struct SubscriptionScope
	{
        private readonly PooledFastList<IEventSubscription> _subscriptions;

        public static SubscriptionScope Create()
        {
            var subscriptions = PoolFastList<IEventSubscription>.Spawn();
            
            return new SubscriptionScope(subscriptions);
        }

        private SubscriptionScope(PooledFastList<IEventSubscription> subscriptions)
        {
            _subscriptions = subscriptions;
        }

        public void Add(IEventSubscription subscription)
        {
            _subscriptions.Add(subscription);
        }

        public void Release()
        {
            foreach (var subscription in _subscriptions)
            {
                subscription.Release();
            }
            
            PoolFastList<IEventSubscription>.Recycle(_subscriptions);
        }
    }
}