using System;

namespace EgorLin.EventBus.Containers
{
	public interface IEventContainer
	{
		void RemoveRaw(Action callback);
	}
}
