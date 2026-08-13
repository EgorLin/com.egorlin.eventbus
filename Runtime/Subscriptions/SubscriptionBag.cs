using EgorLin.EventBus.Pools;

namespace EgorLin.EventBus.Subscriptions
{
	public struct SubscriptionBag
	{
        private readonly PooledFastList<IEventSubscription> _subscriptions;

        public static SubscriptionBag Create()
        {
            var subscriptions = PoolFastList<IEventSubscription>.Spawn();
            
            return new SubscriptionBag(subscriptions);
        }

        private SubscriptionBag(PooledFastList<IEventSubscription> subscriptions)
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