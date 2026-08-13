using System;
using EgorLin.EventBus.Collections;

namespace EgorLin.EventBus.Pools
{
#if ECS_COMPILE_IL2CPP_OPTIONS
	[Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
	 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
	 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
#endif
    internal static class PoolFastList<T>
    {
        private static readonly PoolInternalBase Pool = new(() => new FastList<T>(),
            obj => ((FastList<T>)obj).Clear());

        public static PooledFastList<T> Spawn()
        {
            return new PooledFastList<T>((FastList<T>) Pool.Spawn());
        }

        public static void Recycle(ref FastList<T> instance)
        {
            Pool.Recycle(instance);
            instance = null;
        }

        public static void Recycle(FastList<T> instance)
        {
            Pool.Recycle(instance);
        }
    }

    internal readonly struct PooledFastList<TItem> : IDisposable
    {
        public readonly FastList<TItem> Value;

        public PooledFastList(FastList<TItem> value)
        {
            Value = value;
        }

        public TItem[] data => Value.data;
        public int length => Value.length;

        public void Dispose()
        {
            if (Value != null)
            {
                PoolFastList<TItem>.Recycle(Value);
            }
        }

        public static implicit operator FastList<TItem>(PooledFastList<TItem> pooled)
        {
            return pooled.Value;
        }

        public FastList<TItem>.Enumerator GetEnumerator()
        {
            return Value.GetEnumerator();
        }

        public void Add(TItem item)
        {
            Value.Add(item);
        }

        public void Remove(TItem item)
        {
            Value.Remove(item);
        }

        public void Clear()
        {
            Value.Clear();
        }
    }
}