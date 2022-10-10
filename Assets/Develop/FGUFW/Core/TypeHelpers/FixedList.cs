using System;
using System.Collections.Generic;

namespace FGUFW
{
    public class FixedList<T>
    {
        private List<T[]> _list;
        private int _itemLength;

        private FixedList(){}
        public FixedList(int itemLength = 1024)
        {
            if(itemLength<8)itemLength=1024;
            _itemLength = itemLength;
            _list = new List<T[]>();
            _list.Add(new T[_itemLength]);
        }

        public T this[int index]
        {
            get
            {
                checkIndex(index);
                int arrIdx = index/_itemLength;
                int idx = index%_itemLength;
                return _list[arrIdx][idx];
            }
            set
            {
                checkIndex(index);
                int arrIdx = index/_itemLength;
                int idx = index%_itemLength;
                _list[arrIdx][idx]=value;
            }
        }

        private void checkIndex(int index)
        {
            int arrIdx = index/_itemLength;
            while (arrIdx>=_list.Count)
            {
                _list.Add(new T[_itemLength]);
            }
        }

        public void Clear()
        {
            _list?.Clear();
        }
        
    }
}