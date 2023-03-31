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
    [BurstCompile]
    public partial struct MaterialFlipbookAnimDestroySystem : ISystem
    {
        private EntityQuery _updateEQ;

        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            _updateEQ = new EntityQueryBuilder(Allocator.Temp).WithAll<MaterialFlipbookAnimUpdate>().Build(ref state);
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            state.Dependency.Complete();
            var singletonRW = SystemAPI.GetSingletonRW<MaterialFlipbookAnimEventSingleton>();
            if(singletonRW.ValueRO.Events.Length>0)
            {
                singletonRW.ValueRW.Events.Clear();
            }

            var ecb = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);
            var entitys = _updateEQ.ToEntityArray(Allocator.Temp);
            ecb.RemoveComponent<MaterialFlipbookAnimUpdate>(entitys);
            entitys.Dispose();
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {
            var singleton = SystemAPI.GetSingleton<MaterialFlipbookAnimEventSingleton>();
            singleton.Events.Dispose();
        }


    }
}

