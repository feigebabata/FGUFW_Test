using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Collections;
using Unity.Burst;
using Unity.Transforms;
using NAME_SAPCE;

namespace NAME_SPACE
{
    [BurstCompile]
    partial struct DynamicBufferTestSystem : ISystem
    {


        public void OnUpdate(ref SystemState state)
        {
            NativeList<int> ls = new NativeList<int>(Allocator.Temp);

            for (int i = 0; i < 1000; i++)
            {
                ls.Add(i);
            }
            Debug.Log(ls.Length);
            ls.Dispose();
        }
    }
}
