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
        private uint _bits;

        public uint Value => _bits;

        public bool this[uint i]
        {
            get
            {
                if(i==0)return false;

                return BitHelper.Contains(_bits,i);
            }
            set
            {
                if(i==0)return;

                if(value)
                {
                    _bits = BitHelper.Add(_bits,i);
                }
                else
                {
                    _bits = BitHelper.Sub(_bits,i);
                }
            }
        }

        public bool this[E e]
        {
            get
            {
                return this[(uint)e.GetHashCode()];
            }
            set
            {
                this[(uint)e.GetHashCode()] = value;
            }
        }

        public BitEnums(uint bits)
        {
            _bits = bits;
        }

        public BitEnums(IEnumerable<E> bits)
        {
            _bits = 0;
            foreach (var item in bits)
            {
                this[item]=true;
            }
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

        public unsafe E[] ToArray()
        {
            if(_bits==0)return null;

            var items = Enum.GetValues(typeof(E));
            var indexs = stackalloc int[items.Length];
            int length = 0;
            for (int i = 0; i < items.Length; i++)
            {
                var item = (E)items.GetValue(i);
                if(this[item])
                {
                    indexs[length++] = i;
                }
            }
            var arr = new E[length];
            for (int i = 0; i < length; i++)
            {
                arr[i] = (E)items.GetValue(indexs[i]);
            }
            return arr;
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
            return (int)_bits;
        }

        public override bool Equals(object obj)
        {
            return obj.GetHashCode()==this.GetHashCode();
        }

    }
}