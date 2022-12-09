using System;

namespace FGUFW
{
    public struct Ints8
    {
        public int I0;
        public int I1;
        public int I2;
        public int I3;
        public int I4;
        public int I5;
        public int I6;
        public int I7;

        public int this[int idx]
        {
            get
            {
                if(idx==0)
                {
                    return I0;
                }
                else if(idx==1)
                {
                    return I1;
                }
                else if(idx==2)
                {
                    return I2;
                }
                else if(idx==3)
                {
                    return I3;
                }
                else if(idx==4)
                {
                    return I4;
                }
                else if(idx==5)
                {
                    return I5;
                }
                else if(idx==6)
                {
                    return I6;
                }
                else if(idx==7)
                {
                    return I7;
                }
                throw new IndexOutOfRangeException();
            }
            set
            {
                if(idx==0)
                {
                    I0=value;
                }
                else if(idx==1)
                {
                    I1=value;
                }
                else if(idx==2)
                {
                    I2=value;
                }
                else if(idx==3)
                {
                    I3=value;
                }
                else if(idx==4)
                {
                    I4=value;
                }
                else if(idx==5)
                {
                    I5=value;
                }
                else if(idx==6)
                {
                    I6=value;
                }
                else if(idx==7)
                {
                    I7=value;
                }
                throw new IndexOutOfRangeException();
            }
        }

        public bool InitialValue(int length=8)
        {
            if(length>8)length=8;
            for (int i = 0; i < length; i++)
            {
                if(this[i]==0)return false;
            }
            return true;
        }


    }
}