using System;

namespace FGUFW
{
    public struct Ints8
    {
        public int V0;
        public int V1;
        public int V2;
        public int V3;
        public int V4;
        public int V5;
        public int V6;
        public int V7;

        public int this[int idx]
        {
            get
            {
                if(idx==0)
                {
                    return V0;
                }
                else if(idx==1)
                {
                    return V1;
                }
                else if(idx==2)
                {
                    return V2;
                }
                else if(idx==3)
                {
                    return V3;
                }
                else if(idx==4)
                {
                    return V4;
                }
                else if(idx==5)
                {
                    return V5;
                }
                else if(idx==6)
                {
                    return V6;
                }
                else if(idx==7)
                {
                    return V7;
                }
                throw new IndexOutOfRangeException();
            }
            set
            {
                if(idx==0)
                {
                    V0=value;
                }
                else if(idx==1)
                {
                    V1=value;
                }
                else if(idx==2)
                {
                    V2=value;
                }
                else if(idx==3)
                {
                    V3=value;
                }
                else if(idx==4)
                {
                    V4=value;
                }
                else if(idx==5)
                {
                    V5=value;
                }
                else if(idx==6)
                {
                    V6=value;
                }
                else if(idx==7)
                {
                    V7=value;
                }
                throw new IndexOutOfRangeException();
            }
        }

    }
}