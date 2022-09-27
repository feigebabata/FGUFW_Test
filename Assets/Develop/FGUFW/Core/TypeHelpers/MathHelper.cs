using Unity.Mathematics;
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

        public static float4 VectorRotateTo(float4 dir,float4 targetDir,float maxEulerAngle)
        {
            float angle = math.acos(math.dot(dir,targetDir));
            float maxAngle = math.radians(maxEulerAngle);
            float rotateDelta = math.min(angle,maxAngle);
            float3 axis = math.cross(targetDir.xyz,dir.xyz);
            float4x4 rotateMatrix = float4x4.AxisAngle(axis,rotateDelta);
            dir = math.mul(dir,rotateMatrix);
            return math.normalize(dir);
        }

        /// <summary>
        /// 取插值 在cycle轮后能取到end_Min,end_Max之间的固定插值
        /// </summary>
        /// <param name="cycle"></param>
        /// <param name="end_Min"></param>
        /// <param name="end_Max"></param>
        /// <returns></returns>
        public static float LerpByCycle(int cycle,float end_Min=0.75f,float end_Max=0.85f)
        {
            if (cycle <= 0) return 0;
            if (end_Min<0 || end_Max<0) return 0;
            if (end_Max - end_Min < 0.0001f) return 0;

            float length = 0;
            float t = 0.5f;

            float min = 0;
            float max = 1;

            do
            {
                t = Mathf.Lerp(min, max, 0.5f);
                length = 1;
                for (int i = 0; i < cycle; i++)
                {
                    length -= t * length;
                }
                length = 1 - length;
                if (length >= end_Min && length <= end_Max)
                {
                    return t;
                }
                if (length < end_Min)
                {
                    min = t;
                }
                else
                {
                    max = t;
                }
            }
            while (true);
        }

    }
}