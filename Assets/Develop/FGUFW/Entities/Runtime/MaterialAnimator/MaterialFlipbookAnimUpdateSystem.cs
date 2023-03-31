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
    [UpdateAfter(typeof(MaterialFlipbookAnimPlaySystem))]
    [UpdateBefore(typeof(MaterialFlipbookAnimDestroySystem))]
    [BurstCompile]
    partial struct MaterialFlipbookAnimUpdateSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            EntityQuery eq = new EntityQueryBuilder(Allocator.Temp).WithAll<MaterialFlipbookAnimUpdate,MaterialFlipbookIndex>().Build(ref state);
            state.RequireForUpdate(eq);
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            state.Dependency = new AnimUpdateJob
            {
            }.ScheduleParallel(state.Dependency);
        }

        [BurstCompile]
        partial struct AnimUpdateJob:IJobEntity
        {
            void Execute(in MaterialFlipbookAnimUpdate animUpdate,ref MaterialFlipbookIndex flipbookIndex)
            {
                flipbookIndex.Value = animUpdate.FlipbookIndex;
            }
        }
    }
}

