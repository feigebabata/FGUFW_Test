using System;
using System.Collections.Generic;
using System.Text;

namespace FGUFW
{
    /// <summary>
    /// 位枚举数组 枚举按位赋值 必须>0
    /// </summary>
    public struct BitEnums<E> where E:Enum
    {
        private int _bits;

        public bool this[int i]
        {
            get
            {
                if(i==0)return false;

                return (_bits & i)==i;
            }
            set
            {
                if(i==0)return;

                if(value)
                {
                    _bits = _bits | i;
                }
                else
                {
                    _bits = _bits & (~i);
                }
            }
        }

        public bool this[E e]
        {
            get
            {
                return this[e.GetHashCode()];
            }
            set
            {
                this[e.GetHashCode()] = value;
            }
        }

        public BitEnums(int bits)
        {
            _bits = bits;
        }

        public BitEnums<E> Pares(string text,char separator=',')
        {
            var bitEnums = new BitEnums<E>();
            var items = text.Split(separator);
            foreach (var item in items)
            {
                var i = item.ToEnum<E>();
                bitEnums[i]=true;
            }
            return bitEnums;
        }

        public void Clear()
        {
            _bits = 0;
        }

        public E[] ToArray()
        {
            if(_bits==0)return null;

            var items = Enum.GetValues(typeof(E));
            var list = new List<E>();
            foreach (E item in items)
            {
                if(this[item])
                {
                    list.Add(item);
                }
            }
            return list.ToArray();
        }

        public override string ToString()
        {
            if(_bits==0)return "0";
            StringBuilder sb = new StringBuilder();
            var items = this.ToArray();
            sb.AppendLine(_bits.ToString());
            sb.AppendLine(_bits.ToBitString());
            sb.Append(string.Join<E>(",",items));
            return sb.ToString();
        }

        public override int GetHashCode()
        {
            return _bits;
        }

        public override bool Equals(object obj)
        {
            return obj.GetHashCode()==this.GetHashCode();
        }

    }
}