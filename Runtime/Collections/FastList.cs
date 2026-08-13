using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace EgorLin.EventBus.Collections
{
    [Serializable]
    internal sealed class FastList<T>
    {
        public static FastList<T> Empty = new FastList<T>();

        public T[] data;
        public int length;
        public int capacity;
        public int lastSwappedIndex;

        public EqualityComparer<T> comparer;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public FastList()
        {
            capacity = 4;
            data = new T[capacity];
            length = 0;
            lastSwappedIndex = -1;

            comparer = EqualityComparer<T>.Default;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public FastList(int capacity)
        {
            this.capacity = HashHelpers.GetCapacity(capacity) + 1;
            data = new T[this.capacity];
            length = 0;
            lastSwappedIndex = -1;

            comparer = EqualityComparer<T>.Default;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public FastList(FastList<T> other)
        {
            capacity = other.capacity;
            data = new T[capacity];
            length = other.length;
            lastSwappedIndex = -1;
            Array.Copy(other.data, 0, data, 0, length);

            comparer = other.comparer;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Enumerator GetEnumerator()
        {
            Enumerator e;
            e.length = length;
            e.list = this;
            e.current = default;
            e.index = 0;
            lastSwappedIndex = -1;
            return e;
        }

        public struct ResultSwap
        {
            public int oldIndex;
            public int newIndex;
        }

        public struct Enumerator
        {
            public FastList<T> list;

            public int length;
            public T current;
            public int index;

            public bool MoveNext()
            {
                int lastSwappedIndex = list.lastSwappedIndex;
                if (lastSwappedIndex != -1)
                {
                    length = list.length;
                    int previousIndex = index - 1;
                    if (lastSwappedIndex == previousIndex)
                    {
                        index--;
                    }
                    else if (lastSwappedIndex < previousIndex)
                    {
#if MORPEH_DEBUG
                        throw new InvalidOperationException("Earlier collection items have been modified, this is not allowed");
#endif
                    }
                }

                if (index >= length)
                {
                    return false;
                }

                current = list.data[index++];
                list.lastSwappedIndex = -1;

                return true;
            }

            public T Current
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get => current;
            }
        }
    }
}