namespace EgorLin.EventBus.Subscriptions
{
	public static class EventSubscriptionExtensions
	{
		public static void AddTo(this IEventSubscription subscription, ref SubscriptionScope scope)
		{
			scope.Add(subscription);
		}
	}
}
