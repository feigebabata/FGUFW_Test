
using System;

namespace FGUFW.ECS
{
    /// <summary>
    /// 0:空 1:无操作
    /// </summary>
    public struct FrameOperate
    {
        public uint CMD_0;
        public uint CMD_1;
        public uint CMD_2;
        public uint CMD_3;
        public uint CMD_4;
        public uint CMD_5;
        public uint CMD_6;
        public uint CMD_7;

        public uint this[int idx]
        {
            get
            {
                if(idx==0)
                {
                    return CMD_0;
                }
                else if(idx==1)
                {
                    return CMD_1;
                }
                else if(idx==2)
                {
                    return CMD_2;
                }
                else if(idx==3)
                {
                    return CMD_3;
                }
                else if(idx==4)
                {
                    return CMD_4;
                }
                else if(idx==5)
                {
                    return CMD_5;
                }
                else if(idx==6)
                {
                    return CMD_6;
                }
                else if(idx==7)
                {
                    return CMD_7;
                }
                throw new IndexOutOfRangeException();
            }
            set
            {
                if(idx==0)
                {
                    CMD_0=value;
                }
                else if(idx==1)
                {
                    CMD_1=value;
                }
                else if(idx==2)
                {
                    CMD_2=value;
                }
                else if(idx==3)
                {
                    CMD_3=value;
                }
                else if(idx==4)
                {
                    CMD_4=value;
                }
                else if(idx==5)
                {
                    CMD_5=value;
                }
                else if(idx==6)
                {
                    CMD_6=value;
                }
                else if(idx==7)
                {
                    CMD_7=value;
                }
                else
                {
                    throw new IndexOutOfRangeException();
                }
            }
        }

        public bool Complete(int length)
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