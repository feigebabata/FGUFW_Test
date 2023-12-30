using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Mathematics;

namespace FGUFW
{
    /// <summary>
    /// 空间划分 点模式
    /// 空间由X*Y个格子构成 格子顺序X >> Y
    /// 点在格子内: gridMin >= 点 < gridMax
    /// 空间之外点的所在格子索引为-1
    /// </summary>
    [BurstCompile]
    public struct Space2
    {


        // [BurstCompile]
        public static int IndexOf(float2 point,float2 spaceSize,int2 spaceMaxIndex,float2 spaceCenter)
        {
            int idx = -1;

            float2 spaceMin = spaceCenter-spaceSize/2;
            float2 p = point-spaceMin;

            int idx_x = indexOf(p.x,spaceSize.x,spaceMaxIndex.x);
            if(idx_x==-1)return idx;

            int idx_y = indexOf(p.y,spaceSize.y,spaceMaxIndex.y);
            if(idx_x==-1)return idx;

            idx = idx_x + idx_y*spaceMaxIndex.x;

            return idx;
        }

        // [BurstCompile]
        private static int indexOf(float p,float length,int count)
        {
            int idx = -1;
            if(p<0 || p>=length)return idx;
            idx = (int)(p/length*count);
            return idx;
        }
    }
}