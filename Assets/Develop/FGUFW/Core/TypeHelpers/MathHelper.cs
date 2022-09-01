using UnityEngine;

namespace FGUFW
{
    static public class MathHelper
    {
        /// <summary>
        /// 区间索引
        /// </summary>
        /// <param name="count">区间分段</param>
        /// <param name="length">区间长</param>
        /// <param name="index">位置</param>
        /// <returns></returns>
        static public int IndexOf(int count ,float length,float index)
        {
            float t = count*index/length;
            int idx = (int)t;
            idx = Mathf.Clamp(idx,0,count-1);
            return idx;
        }

        static public int SortInt(float d)
        {
            if(d>0)
            {
                return 1;
            }
            else if(d<0)
            {
                return -1;
            }
            return 0;
        }

        static public int SortInt(double d)
        {
            if(d>0)
            {
                return 1;
            }
            else if(d<0)
            {
                return -1;
            }
            return 0;
        }

        static public int Ceil(this float self)
        {
            return Mathf.CeilToInt(self);
        }

        static public int ToInt32(this float self)
        {
            return (int)self;
        }

        /// <summary>
        /// 向上取整 <-1 && >0 = 0
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        static public int Ceil_Z(this float self)
        {
            if(self<0)
            {
                int idx = (int)self;
                if(self%1!=0)idx--;
                return idx;
            }
            else
            {
                return Mathf.CeilToInt(self);
            }
        }

        /// <summary>
        /// 取余_循环
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        static public int RoundIndex(this int self,int length)
        {
            int idx = (self%length+length)%length;
            return idx;
        }
        
        public static float Distance(float v1,float v2)
        {
            if(v1>v2)
            {
                return v1-v2;
            }
            else
            {
                return v2-v1;
            }
        }
    }
}