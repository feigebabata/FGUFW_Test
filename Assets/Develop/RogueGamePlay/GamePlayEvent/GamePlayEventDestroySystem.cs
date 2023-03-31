using UnityEngine;
using Unity.Entities;
using Unity.Collections;
using Unity.Mathematics;
using Unity.Burst;
using static Unity.Entities.SystemAPI;
using System;
using Unity.Jobs;

namespace RogueGamePlay
{
    [BurstCompile]
    [UpdateBefore(typeof(GamePlayEventCreateSystemGroup))]
    public partial struct GamePlayEventDestroySystem:ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            var ecb = new EntityCommandBuffer(Allocator.Temp);
            ecb.AddComponent(ecb.CreateEntity(),new GamePlayEventSingleton
            {
                Events = new NativeList<GamePlayEventData>(64,Allocator.Persistent)
            });
            ecb.Playback(state.EntityManager);
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            state.Dependency.Complete();
            var singletonRW = SystemAPI.GetSingletonRW<GamePlayEventSingleton>();
            if(singletonRW.ValueRO.Events.Length>0)
            {
                singletonRW.ValueRW.Events.Clear();
            }
        }

    }
}

