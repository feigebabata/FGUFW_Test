using System;
using System.Collections;
using System.Collections.Generic;

namespace FGUFW
{
    //无序的列表
    public sealed class Slice<T>:ICollection<T>
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

        public bool IsReadOnly => false;

        public Slice(int capacity=4)
        {
            Capacity = capacity;
        }

        public Slice(T[] collection)
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
            return true;
        }

        public void Remove(Predicate<T> match)
        {
            var index = FindIndex(match);
            RemoveAt(index);
        }

        public void RemoveAt(int index)
        {
            if(index<0 || index>=_count) return;
            var lastObj = _items[_count];
            _items[index] = lastObj;
            _count--;
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

        public void Clear()
        {
            if(_capacity==0)return;
            Array.Clear(_items,0,_capacity);

        }

        public bool Contains(T item)
        {
            return IndexOf(item) != -1;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            Array.Copy(_items, 0, array, arrayIndex, _count);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new Enumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new Enumerator(this);
        }       
        
        public void Sort(Comparison<T> comparison) 
        {
            if(_count==0)return;
 
            IComparer<T> comparer = new FunctorComparer(comparison);
            Array.Sort(_items, 0, _count, comparer);
        }

        internal sealed class FunctorComparer : IComparer<T> 
        {
            Comparison<T> comparison;
 
            public FunctorComparer(Comparison<T> comparison) 
            {
                this.comparison = comparison;
            }
 
            public int Compare(T x, T y) 
            {
                return comparison(x, y);
            }
        }

        
 
        [Serializable]
        public struct Enumerator : IEnumerator<T>, System.Collections.IEnumerator
        {
            private Slice<T> list;
            private int index;
            // private int version;
            private T current;
 
            internal Enumerator(Slice<T> list) {
                this.list = list;
                index = 0;
                // version = 0;//list._version;
                current = default(T);
            }
 
            public void Dispose() {
            }
 
            public bool MoveNext() {
 
                Slice<T> localList = list;
 
                if (((uint)index < (uint)localList._count)) 
                {                                                     
                    current = localList._items[index];                    
                    index++;
                    return true;
                }
                return MoveNextRare();
            }
 
            private bool MoveNextRare()
            {   
 
                index = list._count + 1;
                current = default(T);
                return false;                
            }
 
            public T Current {
                get {
                    return current;
                }
            }
 
            Object System.Collections.IEnumerator.Current {
                get {
                    if( index == 0 || index == list._count + 1) {
                        //  ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumOpCantHappen);
                    }
                    return Current;
                }
            }
    
            void System.Collections.IEnumerator.Reset() {
                // if (version != list._version) {
                    // ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumFailedVersion);
                // }
                
                index = 0;
                current = default(T);
            }
 
        }
    }
}