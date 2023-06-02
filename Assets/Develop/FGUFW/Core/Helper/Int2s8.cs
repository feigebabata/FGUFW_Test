using System;
using Unity.Mathematics;

namespace FGUFW
{
    public struct Int2s8
    {
        public int2 V0;
        public int2 V1;
        public int2 V2;
        public int2 V3;
        public int2 V4;
        public int2 V5;
        public int2 V6;
        public int2 V7;

        public int2 this[int idx]
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
                else
                {
                    throw new IndexOutOfRangeException();
                }
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
                else
                {
                    throw new IndexOutOfRangeException();
                }
            }
        }

    }
}