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
            foreach (var (buffers,dbt,entity) in SystemAPI.Query<DynamicBuffer<TestBuffer>,DynamicBufferTest>().WithEntityAccess())
            {
                Debug.Log(entity);
            }
        }
    }
}
