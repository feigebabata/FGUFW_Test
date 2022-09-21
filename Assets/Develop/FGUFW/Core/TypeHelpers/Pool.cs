using System;
using System.Collections;
using System.Collections.Generic;

namespace FGUFW
{
    public sealed class Pool<T> : IEnumerable<T>
    {
        public int Capacity=>_cache.Capacity;
        private List<T> _cache;
        private int _cacheIndex;
        private bool _limited;
        private Func<T, T> _copyT;
        private T _template;
        private Action<bool,T> _recycle;

        private Pool(){}

        public Pool(T template,int capacity,bool limited,Func<T,T> copyT,Action<bool,T> recycle=null)
        {
            if(capacity<1)capacity=1;
            _cache = new List<T>(capacity);
            _limited = limited;
            _copyT = copyT;
            _template = template;
            _recycle = recycle;
        }

        public void ReCycle(T t)
        {
            int index = _cache.IndexOf(t);
            if(index!=-1 && index<_cacheIndex)
            {
                _cacheIndex--;
                var temp = _cache[_cacheIndex];
                _cache[_cacheIndex] = t;
                _cache[index] = temp;

            }
            if(_recycle!=null)_recycle(index!=-1,t);
        }

        public void Clear()
        {
            _cache.Clear();
            _cacheIndex = 0;
        }

        public T Get()
        {
            if(_cacheIndex<_cache.Count)
            {
                return _cache[_cacheIndex++];
            }
            else
            {
                if(_limited)
                {
                    return _copyT(_template);
                }
                else
                {
                    var t = _copyT(_template);
                    _cache.Add(t);
                    _cacheIndex++;
                    return t;
                }
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _cache.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _cache.GetEnumerator();
        }
    }
}