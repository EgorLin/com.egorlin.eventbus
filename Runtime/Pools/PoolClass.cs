namespace EgorLin.EventBus.Pools
{
    internal static class PoolClass<T> where T : class, new()
    {
        public static readonly PoolInternalBase Pool = new(() => new T(), null);

        public static T Spawn()
        {
            return (T) Pool.Spawn();
        }

        public static void Recycle(ref T instance)
        {
            Pool.Recycle(instance);
            instance = null;
        }

        public static void Recycle(T instance)
        {
            Pool.Recycle(instance);
        }
    }
}