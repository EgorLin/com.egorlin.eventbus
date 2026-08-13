using System;
using System.Runtime.CompilerServices;

namespace EgorLin.EventBus.Collections
{
    internal static class FastListExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Expand<T>(this FastList<T> list) where T : unmanaged
        {
            ArrayHelpers.Grow(ref list.data, list.capacity = HashHelpers.GetCapacity(list.capacity) + 1);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Resize<T>(this FastList<T> list, int newCapacity) where T : unmanaged
        {
            ArrayHelpers.Grow(ref list.data, list.capacity = HashHelpers.GetCapacity(newCapacity - 1) + 1);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Add<T>(this FastList<T> list)
        {
            int index = list.length;
            if (++list.length == list.capacity)
            {
                ArrayHelpers.Grow(ref list.data, list.capacity = HashHelpers.GetCapacity(list.capacity) + 1);
            }

            return index;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Add<T>(this FastList<T> list, T value)
        {
            int index = list.length;
            if (++list.length == list.capacity)
            {
                ArrayHelpers.Grow(ref list.data, list.capacity = HashHelpers.GetCapacity(list.capacity) + 1);
            }

            list.data[index] = value;
            return index;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static FastList<T> WithElement<T>(this FastList<T> list, T value)
        {
            list.Add(value);
            return list;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddListRange<T>(this FastList<T> list, FastList<T> other)
        {
            if (other.length > 0)
            {
                int newSize = list.length + other.length;

                if (newSize > list.capacity)
                {
                    ArrayHelpers.Grow(ref list.data, list.capacity = HashHelpers.GetCapacity(newSize - 1) + 1);
                }

                Array.Copy(other.data, 0, list.data, list.length, other.length);

                list.length += other.length;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Swap<T>(this FastList<T> list, int source, int destination)
        {
            list.data[destination] = list.data[source];
            list.lastSwappedIndex = destination;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddRange<T>(this FastList<T> list, FastList<T> listSource)
        {
            foreach (T item in listSource)
            {
                list.Add(item);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Contains<T>(this FastList<T> list, T value)
        {
            return list.IndexOf(value) != -1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int IndexOf<T>(this FastList<T> list, T value)
        {
            for (int i = 0, length = list.length; i < length; i++)
            {
                if (list.comparer.Equals(value, list.data[i]))
                {
                    return i;
                }
            }

            return -1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Remove<T>(this FastList<T> list, T value)
        {
            int indexOf = list.IndexOf(value);

            if (indexOf != -1)
            {
                list.RemoveAt(indexOf);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void RemoveSave<T>(this FastList<T> list, T value)
        {
            int index = list.IndexOf(value);
            if (index < 0)
            {
                return;
            }

            list.RemoveAt(index);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void RemoveSwap<T>(this FastList<T> list, T value, out FastList<T>.ResultSwap swap)
        {
            list.RemoveAtSwap(list.IndexOf(value), out swap);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void RemoveSwapSave<T>(this FastList<T> list, T value, out FastList<T>.ResultSwap swap)
        {
            int index = list.IndexOf(value);
            if (index < 0)
            {
                swap = default;
                return;
            }

            list.RemoveAtSwap(index, out swap);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void RemoveAt<T>(this FastList<T> list, int index)
        {
            --list.length;
            if (index < list.length)
            {
                Array.Copy(list.data, index + 1, list.data, index, list.length - index);
            }

            list.data[list.length] = default;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool RemoveAtSwap<T>(this FastList<T> list, int index, out FastList<T>.ResultSwap swap)
        {
            if (list.length-- > 1)
            {
                swap.oldIndex = list.length;
                swap.newIndex = index;

                list.data[swap.newIndex] = list.data[swap.oldIndex];
                list.data[swap.oldIndex] = default;
                list.lastSwappedIndex = index;
                return true;
            }

            list.lastSwappedIndex = -1;
            swap = default;
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool RemoveAtSwap<T>(this FastList<T> list, int index, out T newValue)
        {
            if (list.length-- > 1)
            {
                int oldIndex = list.length;
                newValue = list.data[index] = list.data[oldIndex];
                list.lastSwappedIndex = index;
                return true;
            }

            list.lastSwappedIndex = -1;
            newValue = default;
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Clear<T>(this FastList<T> list)
        {
            if (list.length <= 0)
            {
                return;
            }

            Array.Clear(list.data, 0, list.length);
            list.length = 0;
            list.lastSwappedIndex = -1;
        }

        //todo rework
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Sort<T>(this FastList<T> list, object index)
        {
            Array.Sort(list.data, 0, list.length, null);
        }

        //todo rework
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Sort<T>(this FastList<T> list, int index, int len)
        {
            Array.Sort(list.data, index, len, null);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T[] ToArray<T>(this FastList<T> list)
        {
            var newArray = new T[list.length];
            Array.Copy(list.data, 0, newArray, 0, list.length);
            return newArray;
        }

        public static void Shuffle<T>(this FastList<T> list)
        {
            var random = new System.Random(DateTime.Now.Millisecond);

            for (int i = list.length - 1; i >= 1; i--)
            {
                int j = random.Next(i + 1);
                list.Swap(i, j);
            }
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ReadOnlySpan<T> AsReadOnlySpan<T>(this FastList<T> list)
        {
            return new ReadOnlySpan<T>(list.data, 0, list.length);
        }
    }
}