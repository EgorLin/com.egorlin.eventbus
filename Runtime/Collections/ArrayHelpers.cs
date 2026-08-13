using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace EgorLin.EventBus.Collections
{
    internal static class ArrayHelpers
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Grow<T>(ref T[] array, int newSize)
        {
            var newArray = new T[newSize];
            Array.Copy(array, 0, newArray, 0, array.Length);
            array = newArray;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int IndexOf<T>(T[] array, T value, EqualityComparer<T> comparer)
        {
            for (int i = 0, length = array.Length; i < length; ++i)
            {
                if (comparer.Equals(array[i], value))
                {
                    return i;
                }
            }

            return -1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe int IndexOfUnsafeInt(int* array, int length, int value)
        {
            var i = 0;
            for (int* current = array, len = array + length; current < len; ++current)
            {
                if (*current == value)
                {
                    return i;
                }

                ++i;
            }

            return -1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void InsertionSort(this int[] array, int offset, int length)
        {
            int r = offset + length;
            for (int i = offset + 1; i < r; i++)
            {
                ref int ie = ref array[i];
                ref int im1e = ref array[i - 1];
                if (ie < im1e)
                {
                    int currentElement = ie;
                    ie = im1e;
                    int j = i - 1;
                    for (; j > offset && currentElement < array[j - 1]; j--)
                    {
                        array[j] = array[j - 1];
                    }

                    array[j] = currentElement;
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void InsertionSort(this long[] array, int offset, int length)
        {
            int r = offset + length;
            for (int i = offset + 1; i < r; i++)
            {
                ref long ie = ref array[i];
                ref long im1e = ref array[i - 1];
                if (ie < im1e)
                {
                    long currentElement = ie;
                    ie = im1e;
                    int j = i - 1;
                    for (; j > offset && currentElement < array[j - 1]; j--)
                    {
                        array[j] = array[j - 1];
                    }

                    array[j] = currentElement;
                }
            }
        }
    }
}