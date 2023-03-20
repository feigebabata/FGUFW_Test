using UnityEngine;
using Unity.Entities;
using Unity.Collections;
using Unity.Mathematics;
using Unity.Burst;
using static Unity.Entities.SystemAPI;
using System;
using Unity.Jobs;

namespace FGUFW.ECS_SpriteAnimator
{
    [UpdateAfter(typeof(SpriteAnimaionEventCreateSystem))]
    [UpdateInGroup(typeof(SpriteAnimatorSystemGroup))]
    [BurstCompile]
    public partial struct SpriteAnimaionEventDestorySystem:ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            state.Dependency.Complete();
            var singletonRW = SystemAPI.GetSingletonRW<SpriteAnimEventsSingleton>();
            if(singletonRW.ValueRO.Events.Length>0)
            {
                // Debug.Log($"Destory {singletonRW.ValueRW.Events.Length}");
                singletonRW.ValueRW.Events.Clear();
            }
        }

    }
}
