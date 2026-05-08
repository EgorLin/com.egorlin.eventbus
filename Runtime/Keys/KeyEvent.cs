using EgorLin.EventBus.Utils;

namespace EgorLin.EventBus.Keys
{
	public readonly struct KeyEvent<T>
	{
		public readonly int Id;
		
		public KeyEvent(string value)
		{
			Id = HashUtils.StringToHash32(value);
		}

		public KeyEvent(int id)
		{
			Id = id;
		}
	}
	
	public readonly struct KeyEvent
	{
		public readonly int Id;
		
		public KeyEvent(string value)
		{
			Id = HashUtils.StringToHash32(value);
		}

		public KeyEvent(int id)
		{
			Id = id;
		}
	}
}