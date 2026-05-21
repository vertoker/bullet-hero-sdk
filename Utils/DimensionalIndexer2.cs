using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace BH.SDK.Utils
{
    /// <summary>
    /// Прокси-класс, предназначенный для работы с обычными массивами как в 2-мерными.
    /// Специальная реализация только для 2 измерений в целях оптимизации
    /// </summary>
    public readonly struct DimensionalIndexer2 : IEnumerable<int>, IEnumerable<DimensionalIndexer2.IndexTuple>
    {
        public int LengthWidth { get; }
        public int LengthHeight { get; }
        public int Length { get; }

        public DimensionalIndexer2(int lengthWidth, int lengthHeight)
        {
            LengthWidth = lengthWidth; // aka width
            LengthHeight = lengthHeight; // aka height
            Length = lengthWidth * lengthHeight;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int GetIndex(int indexWidth, int indexHeight)
        {
            return indexHeight * LengthWidth + indexWidth;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int GetIndex(IndexTuple indexTuple)
        {
            return indexTuple.IndexHeight * LengthWidth + indexTuple.IndexWidth;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetIndex(int indexWidth, int indexHeight, int lengthWidth)
        {
            return indexHeight * lengthWidth + indexWidth;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetIndex(IndexTuple indexTuple, int lengthWidth)
        {
            return indexTuple.IndexHeight * lengthWidth + indexTuple.IndexWidth;
        }
        
        public (int, int) GetIndexes(int index)
        {
            var indexHeight = index / LengthWidth;
            var indexWidth = index - indexHeight * LengthWidth;
            return (indexWidth, indexHeight);
        }
        public static (int, int) GetIndexes(int index, int lengthWidth)
        {
            var indexHeight = index / lengthWidth;
            var indexWidth = index - indexHeight * lengthWidth;
            return (indexWidth, indexHeight);
        }
        
        // IEnumerators
        
        public IEnumerator<int> Enumerate()
        {
            for (var index = 0; index < Length; index++)
                yield return index;
        }
        public static IEnumerator<int> Enumerate(int lengthWidth, int lengthHeight)
        {
            var length = lengthWidth * lengthHeight;
            for (var index = 0; index < length; index++)
                yield return index;
        }
        
        public IEnumerable<(int, int)> Enumerate2()
        {
            for (var indexHeight = 0; indexHeight < LengthHeight; indexHeight++)
            for (var indexWidth = 0; indexWidth < LengthWidth; indexWidth++)
                yield return (indexWidth, indexHeight);
        }
        public static IEnumerable<(int, int)> Enumerate2(int lengthWidth, int lengthHeight)
        {
            for (var indexHeight = 0; indexHeight < lengthHeight; indexHeight++)
            for (var indexWidth = 0; indexWidth < lengthWidth; indexWidth++)
                yield return (indexWidth, indexHeight);
        }
        
        public static IEnumerable<(T, int)> Enumerate<T>(IEnumerable<T> collection)
        {
            using var enumerator = collection.GetEnumerator();
            var counter = 0;
            
            while (enumerator.MoveNext())
                yield return (enumerator.Current, counter++);
        }
        
        public IEnumerable<(T, int, int)> Enumerate2<T>(IEnumerable<T> collection)
        {
            using var enumerator = collection.GetEnumerator();
            int counter1 = 0, counter2 = 0;

            while (enumerator.MoveNext())
            {
                yield return (enumerator.Current, counter1, counter2);
                
                counter1++;
                if (counter1 == LengthWidth)
                {
                    counter2++;
                    counter1 = 0;
                }
            }
        }
        public IEnumerable<(T, int, int)> Enumerate2<T>(IEnumerable<T> collection, int lengthWidth)
        {
            using var enumerator = collection.GetEnumerator();
            int counter1 = 0, counter2 = 0;

            while (enumerator.MoveNext())
            {
                yield return (enumerator.Current, counter1, counter2);
                
                counter1++;
                if (counter1 == lengthWidth)
                {
                    counter2++;
                    counter1 = 0;
                }
            }
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Enumerator GetEnumerator() => new(LengthWidth);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Enumerator2 GetEnumerator2() => new(LengthWidth, LengthHeight);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        IEnumerator<IndexTuple> IEnumerable<IndexTuple>.GetEnumerator() => new Enumerator2(LengthWidth, LengthHeight);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        IEnumerator<int> IEnumerable<int>.GetEnumerator() => new Enumerator(Length);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        IEnumerator IEnumerable.GetEnumerator() => new Enumerator(Length);
        
        public struct Enumerator : IEnumerator<int>
        {
            private readonly int _length;

            public int Current { get; private set; }

            public Enumerator(int length)
            {
                _length = length;
                Current = -1;
            }

            public bool MoveNext()
            {
                return ++Current < _length;
            }

            public void Reset() => Current = -1;
            object IEnumerator.Current => Current;
            public void Dispose() { }
        }

        public struct IndexTuple
        {
            public int IndexWidth;
            public int IndexHeight;
            
            public IndexTuple(int indexWidth, int indexHeight)
            {
                IndexWidth = indexWidth;
                IndexHeight = indexHeight;
            }
        }
        
        public struct Enumerator2 : IEnumerator<IndexTuple>
        {
            private readonly int _length1;
            private readonly int _length;
            
            private int _index;

            public IndexTuple Current { get; private set; }

            public Enumerator2(int length1, int length2)
            {
                _length1 = length1;
                _length = length1 * length2;
                _index = -1;
                Current = default;
            }

            public bool MoveNext()
            {
                if (++_index >= _length)
                    return false;

                var indexHeight = _index / _length1;
                var indexWidth = _index - indexHeight * _length1;
                Current = new IndexTuple(indexWidth, indexHeight);
                return true;
            }

            public void Reset() => _index = -1;
            object IEnumerator.Current => Current;
            public void Dispose() { }
        }
    }
}