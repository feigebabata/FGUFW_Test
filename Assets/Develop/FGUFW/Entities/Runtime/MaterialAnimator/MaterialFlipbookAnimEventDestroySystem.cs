using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Rendering;
using UnityEngine;
using Unity.Mathematics;
using System;
using Unity.Collections;
using UnityEngine.Rendering;
using Unity.Burst;

namespace FGUFW.Entities
{

    [UpdateInGroup(typeof(MaterialFlipbookAnimatorSystemGroup))]
    [UpdateAfter(typeof(MaterialFlipbookAnimEventCreateSystem))]
    [BurstCompile]
    public partial struct MaterialFlipbookAnimEventDestroySystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            state.Dependency.Complete();
            var singletonRW = SystemAPI.GetSingletonRW<MaterialFlipbookAnimEventSingleton>();
            if(singletonRW.ValueRO.Events.Length>0)
            {
                singletonRW.ValueRW.Events.Clear();
            }
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {
            var singleton = SystemAPI.GetSingleton<MaterialFlipbookAnimEventSingleton>();
            singleton.Events.Dispose();
        }

    }
}

