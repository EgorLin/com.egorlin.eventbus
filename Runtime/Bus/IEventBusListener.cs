using System;
using EgorLin.EventBus.Keys;
using EgorLin.EventBus.Subscriptions;

namespace EgorLin.EventBus.Bus
{
	public interface IEventBusListener
	{
		IEventSubscription Subscribe<T>(KeyEvent<T> key, Action<T> callback);
		IEventSubscription Subscribe(KeyEvent key, Action callback);
	}
}
