namespace EgorLin.EventBus.Subscriptions
{
	public static class EventSubscriptionExtensions
	{
		public static void AddTo(this IEventSubscription subscription, ref SubscriptionBag bag)
		{
			bag.Add(subscription);
		}
	}
}
