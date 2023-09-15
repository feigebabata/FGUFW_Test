using System;

namespace FGUFW
{
    public struct Vals16<T> where T : unmanaged
    {
        public const int Capacity = 16;

        public int Length{get;private set;}

        public T V0;
        public T V1;
        public T V2;
        public T V3;
        public T V4;
        public T V5;
        public T V6;
        public T V7;
        public T V8;
        public T V9;
        public T V10;
        public T V11;
        public T V12;
        public T V13;
        public T V14;
        public T V15;

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

                else if(idx==8) return V8;

                else if(idx==9) return V9;

                else if(idx==10) return V10;

                else if(idx==11) return V11;

                else if(idx==12) return V12;

                else if(idx==13) return V13;

                else if(idx==14) return V14;

                else if(idx==15) return V15;

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

                else if(idx==8) V8=value;

                else if(idx==9) V9=value;

                else if(idx==10) V10=value;

                else if(idx==11) V11=value;

                else if(idx==12) V12=value;

                else if(idx==13) V13=value;

                else if(idx==14) V14=value;

                else if(idx==15) V15=value;

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