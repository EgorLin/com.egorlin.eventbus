using EgorLin.EventBus.Keys;

namespace EgorLin.EventBus.Bus
{
	public interface IEventBusPublisher
	{
		void Publish<T>(KeyEvent<T> key, T value);
		void Publish(KeyEvent key);
	}
}
