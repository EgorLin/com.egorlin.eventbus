using System;
using System.Runtime.CompilerServices;

namespace EgorLin.EventBus.Containers
{
	internal sealed class EventContainer<T> : IEventContainer
	{
		private Action<T>[] _listeners = new Action<T>[1];
		private int _countListeners;

		private Action<T>[] _listenersInvoking = Array.Empty<Action<T>>();
		private int _countListenersInvoking;

		private int _depthInvokingListeners;

		public void Add(Action<T> callback)
		{
			EnsureListenersCapacity();
			
			_listeners[_countListeners] = callback;
			
			_countListeners += 1;
		}

		public void Remove(Action<T> callback)
		{
			for (var indexListener = _countListeners - 1; indexListener >= 0; indexListener--)
			{
				if (_listeners[indexListener] != callback)
				{
					continue;
				}

				_countListeners -= 1;
				_listeners[indexListener] = _listeners[_countListeners];
				_listeners[_countListeners] = null;
				
				return;
			}
			
			if (_depthInvokingListeners > 0)
			{
				for (int indexInvoking = _countListenersInvoking - 1; indexInvoking >= 0; indexInvoking--)
				{
					if (_listenersInvoking[indexInvoking] != callback)
					{
						continue;
					}
					
					_listenersInvoking[indexInvoking] = null;

					return;
				}
			}
		}

		public void RemoveRaw(Action callback)
		{
			if (callback is Action<T> typed)
			{
				Remove(typed);
			}
		}

		public void Invoke(T value)
		{
			SwapToInvoking();
			
			_depthInvokingListeners += 1;
			
			try
			{
				for (var indexInvoking = 0; indexInvoking < _countListenersInvoking; indexInvoking++)
				{
					_listenersInvoking[indexInvoking]?.Invoke(value);
				}
			}
			finally
			{
				_depthInvokingListeners -= 1;
				
				if (_depthInvokingListeners <= 0)
				{
					SwapFromInvoking();
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void SwapToInvoking()
		{
			(_listeners, _listenersInvoking) = (_listenersInvoking, _listeners);
			(_countListeners, _countListenersInvoking) = (0, _countListeners);

			if (_listeners.Length < _countListenersInvoking)
			{
				_listeners = new Action<T>[_countListenersInvoking];
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void SwapFromInvoking()
		{
			for (int indexInvoking = 0; indexInvoking < _countListenersInvoking; indexInvoking++)
			{
				if (_listenersInvoking[indexInvoking] == null)
				{
					continue;
				}
				
				EnsureListenersCapacity();
				
				_listeners[_countListeners] = _listenersInvoking[indexInvoking];
				_listenersInvoking[indexInvoking] = null;

				_countListeners += 1;
			}
			
			_countListenersInvoking = 0;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void EnsureListenersCapacity()
		{
			if (_countListeners < _listeners.Length)
			{
				return;
			}
			
			var grown = new Action<T>[_listeners.Length * 2];
			Array.Copy(_listeners, grown, _countListeners);
			_listeners = grown;
		}
	}
	
	internal sealed class EventContainer : IEventContainer
	{
		private Action[] _listeners = new Action[1];
		private int _countListeners;

		private Action[] _listenersInvoking = Array.Empty<Action>();
		private int _countListenersInvoking;

		private int _depthInvokingListeners;

		public void Add(Action callback)
		{
			EnsureListenersCapacity();
			
			_listeners[_countListeners] = callback;

			_countListeners += 1;
		}

		public void Remove(Action callback)
		{
			for (var indexListener = _countListeners - 1; indexListener >= 0; indexListener--)
			{
				if (_listeners[indexListener] != callback)
				{
					continue;
				}
				
				_countListeners -= 1;
				
				_listeners[indexListener] = _listeners[_countListeners];
				_listeners[_countListeners] = null;
				
				return;
			}

			if (_depthInvokingListeners > 0)
			{
				for (int indexInvoking = _countListenersInvoking - 1; indexInvoking >= 0; indexInvoking--)
				{
					if (_listenersInvoking[indexInvoking] != callback)
					{
						continue;
					}
					
					_listenersInvoking[indexInvoking] = null;

					return;
				}
			}
		}

		public void RemoveRaw(Action callback)
		{
			Remove(callback);
		}

		public void Invoke()
		{
			SwapToInvoking();
			
			_depthInvokingListeners += 1;
			
			try
			{
				for (int indexInvoking = 0; indexInvoking < _countListenersInvoking; indexInvoking++)
				{
					_listenersInvoking[indexInvoking]?.Invoke();
				}
			}
			finally
			{
				_depthInvokingListeners -= 1;
				
				if (_depthInvokingListeners <= 0)
				{
					SwapFromInvoking();
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void SwapToInvoking()
		{
			(_listeners, _listenersInvoking) = (_listenersInvoking, _listeners);
			(_countListeners, _countListenersInvoking) = (0, _countListeners);

			if (_listeners.Length < _countListenersInvoking)
			{
				_listeners = new Action[_countListenersInvoking];
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void SwapFromInvoking()
		{
			for (var indexInvoking = 0; indexInvoking < _countListenersInvoking; indexInvoking++)
			{
				if (_listenersInvoking[indexInvoking] == null)
				{
					continue;
				}
				
				EnsureListenersCapacity();
				
				_listeners[_countListeners] = _listenersInvoking[indexInvoking];
				_listenersInvoking[indexInvoking] = null;

				_countListeners += 1;
			}
			
			_countListenersInvoking = 0;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void EnsureListenersCapacity()
		{
			if (_countListeners < _listeners.Length)
			{
				return;
			}
			
			var grown = new Action[_listeners.Length * 2];
			Array.Copy(_listeners, grown, _countListeners);
			
			_listeners = grown;
		}
	}
}