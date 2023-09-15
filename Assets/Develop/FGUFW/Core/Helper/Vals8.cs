using System;

namespace FGUFW
{
    public struct Vals8<T> where T : unmanaged
    {
        public const int Capacity = 8;

        public int Length{get;private set;}

        public T V0;
        public T V1;
        public T V2;
        public T V3;
        public T V4;
        public T V5;
        public T V6;
        public T V7;

        public T this[int idx]
        {
            get
            {
                if(idx<0 || idx>=Length) throw new IndexOutOfRangeException();

                if(idx==0) return V0;

                else if(idx==1) return V1;

                else if(idx==2) return V2;

                else if(idx==3) return V3;

                else if(idx==4) return V4;

                else if(idx==5) return V5;

                else if(idx==6) return V6;

                else if(idx==7) return V7;

                throw new IndexOutOfRangeException();
            }
            set
            {
                if(idx<0 || idx>=Length) throw new IndexOutOfRangeException();

                if(idx==0) V0=value;

                else if(idx==1) V1=value;

                else if(idx==2) V2=value;

                else if(idx==3) V3=value;

                else if(idx==4) V4=value;

                else if(idx==5) V5=value;

                else if(idx==6) V6=value;

                else if(idx==7) V7=value;

                else throw new IndexOutOfRangeException();
            }
        }

        public int IndexOf(T val)
        {
            for (int i = 0; i < this.Length; i++)
            {
                if(this[i].Equals(val))
                {
                    return i;
                }
            }
            return -1;
        }

        public void Add(T val)
        {
            if(Length==Capacity) throw new IndexOutOfRangeException();
            this[Length++] = val;
        }

        public void Remove(T val)
        {
            var idx = IndexOf(val);
            RemoveAt(idx);
        }

        public void RemoveAt(int index)
        {
            if(index<0 || index>=Length) throw new IndexOutOfRangeException();
            this[index] = this[Length-1];
            Length--;
        }

    }
}