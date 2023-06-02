using System;
using Unity.Mathematics;

namespace FGUFW
{
    public struct Int2s16
    {
        public int2 V0;
        public int2 V1;
        public int2 V2;
        public int2 V3;
        public int2 V4;
        public int2 V5;
        public int2 V6;
        public int2 V7;
        public int2 V8;
        public int2 V9;
        public int2 V10;
        public int2 V11;
        public int2 V12;
        public int2 V13;
        public int2 V14;
        public int2 V15;

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
                else if(idx==8)
                {
                    return V8;
                }
                else if(idx==9)
                {
                    return V9;
                }
                else if(idx==10)
                {
                    return V10;
                }
                else if(idx==11)
                {
                    return V11;
                }
                else if(idx==12)
                {
                    return V12;
                }
                else if(idx==13)
                {
                    return V13;
                }
                else if(idx==14)
                {
                    return V14;
                }
                else if(idx==15)
                {
                    return V15;
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
                else if(idx==8)
                {
                    V8=value;
                }
                else if(idx==9)
                {
                    V9=value;
                }
                else if(idx==10)
                {
                    V10=value;
                }
                else if(idx==11)
                {
                    V11=value;
                }
                else if(idx==12)
                {
                    V12=value;
                }
                else if(idx==13)
                {
                    V13=value;
                }
                else if(idx==14)
                {
                    V14=value;
                }
                else if(idx==15)
                {
                    V15=value;
                }
                else
                {
                    throw new IndexOutOfRangeException();
                }
            }
        }

    }
}