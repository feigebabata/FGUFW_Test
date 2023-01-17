using System;
using System.Collections;
using System.Collections.Generic;

namespace FGUFW
{
    /// <summary>
    /// 无序列表 避免移除时后面整体向前复制,但打乱了顺序
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class UnorderedList<T>:IDisposable
    {
        private const int EXPAND = 1024;

        private T[] _items;

        private int _capacity;
        public int Capacity
        {
            get=>_capacity;
            set
            {
                var newCapacity = Math.Max(0,value);
                if(_capacity==newCapacity)return;
                if(_capacity==0)
                {
                    _items = new T[newCapacity];
                }
                else if(newCapacity==0)
                {
                    _items=null;
                }
                else
                {
                    var newItems = new T[newCapacity];
                    Array.Copy(_items,newItems,_capacity);
                    _items = newItems;
                }
                _capacity = newCapacity;
            }
        }

        private int _count;
        public int Count=>_count;

        public UnorderedList(int capacity=4)
        {
            Capacity = capacity;
        }

        public UnorderedList(T[] collection)
        {
            _items = collection;
            _capacity = collection.Length;
            _count = _capacity;
        }

        private void ensureCapacity() 
        {
            if(_capacity==0)_capacity=4;
            var expand = Math.Min(_capacity,EXPAND);
            var newCapacity = _capacity+expand;
            Capacity = newCapacity;
        }

        public T this[int index]
        {
            get
            {
                if(index<0 || index>=_count) throw new IndexOutOfRangeException();
                return _items[index];
            }
            set
            {
                if(index<0 || index>=_count) throw new IndexOutOfRangeException();
                _items[index] = value;
            }
        }

        public T[] this[int start,int end]
        {
            get
            {
                if(start>end)return null;
                if(start<0 || start>=_count || end<0 || start>=end) throw new IndexOutOfRangeException();
                var length = end - start+1;
                var array = new T[length];
                Array.Copy(_items,start,array,0,length);
                return array;
            }
        }

        public void Add(T tObj)
        {
            if(_count>=_capacity)ensureCapacity();
            _items[_count] = tObj;
            _count++;
        }

        public T GetNextCache()
        {
            if(_count>=_capacity)ensureCapacity();
            T tObj = _items[_count];
            _count++;

            return tObj;
        }

        public void SetLast(T tObj)
        {
            _items[_count-1] = tObj;
        }

        public bool Remove(T tObj)
        {
            int index = IndexOf(tObj);
            return RemoveAt(index);
        }

        public void Remove(Predicate<T> match)
        {
            var index = FindIndex(match);
            RemoveAt(index);
        }

        public bool RemoveAt(int index)
        {
            if(index<0 || index>=_count) return false;
            var lastObj = _items[_count];
            _items[index] = lastObj;
            _count--;
            return true;
        }

        public int IndexOf(T tObj)
        {
            return Array.IndexOf<T>(_items,tObj,0,_count);
        }

        public int FindIndex(Predicate<T> match)
        {
            return Array.FindIndex<T>(_items,0,_count,match);
        }

        public T Find(Predicate<T> match)
        {
            int index = FindIndex(match);
            if(index==-1)return default(T);
            return _items[index];
        }

        public bool Contains(T item)
        {
            return IndexOf(item) != -1;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            Array.Copy(_items, 0, array, arrayIndex, _count);
        }

        public void Sort(Comparison<T> comparer)
        {
            Array.Sort<T>(_items,0,_count,new SortComparer<T>(comparer));
        }

        public void AddRange(ICollection<T> collection,int index)
        {
            int maxLength = index + collection.Count;
            if(Capacity<maxLength)
            {
                Capacity=maxLength;
            }
            if(_count<maxLength)
            {
                _count=maxLength;
            }
            collection.CopyTo(_items,index);
        }

        public void Clear()
        {
            _count = 0;
        }

        public void Dispose()
        {
            Clear();
            if(_capacity==0)return;
            Array.Clear(_items,0,_capacity);
        }

        public struct SortComparer<K> : IComparer<K>
        {
            private Comparison<K> _comparison;

            public SortComparer(Comparison<K> comparison)
            {
                _comparison = comparison;
            }
            public int Compare(K x, K y)
            {
                return _comparison(x,y);
            }
        }
    }
}