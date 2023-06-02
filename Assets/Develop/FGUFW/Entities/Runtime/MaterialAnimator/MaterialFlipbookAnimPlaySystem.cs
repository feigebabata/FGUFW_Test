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

#pragma warning disable CS0282

namespace FGUFW.Entities
{
    struct MaterialFlipbookAnimUpdate : IComponentData,IEnableableComponent
    {
        // public BatchMaterialID MaterialID;
        // public int Start;
        // public int Length;
        // public int FrameCount;
        public int FlipbookIndex;
    }

    [UpdateInGroup(typeof(MaterialFlipbookAnimatorSystemGroup))]
    [UpdateAfter(typeof(MaterialBakerSystem))]
    [BurstCompile]
    partial struct MaterialFlipbookAnimPlaySystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            EntityQuery eq = new EntityQueryBuilder(Allocator.Temp).WithAll<MaterialFlipbookAnimator,MaterialFlipbookAnimationData>().Build(ref state);
            state.RequireForUpdate(eq);
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            if(SystemAPI.Time.DeltaTime==0)return;

            var ecb = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);

            state.Dependency = new AnimPlayJob
            {
                DeltaTime = SystemAPI.Time.DeltaTime,
                ECB = ecb.AsParallelWriter(),
            }
            .ScheduleParallel(state.Dependency);
        }

        [BurstCompile]
        partial struct AnimPlayJob:IJobEntity
        {
            [ReadOnly]public float DeltaTime;
            public EntityCommandBuffer.ParallelWriter ECB;

            void Execute([ChunkIndexInQuery] int chunkInQueryIndex,Entity entity,in MaterialFlipbookAnimationData animData,ref MaterialFlipbookAnimator animator)
            {
                if(animator.Speed==0)return;
                if(!animator.Loop && animator.FrameIndex>=animData.Length-1)return;

                // float frameTime = animData.Time/animData.Length;
                // float deltaTime = animator.Time - animator.FrameIndex*frameTime + DeltaTime*animator.Speed;
                animator.Time = (animator.Time + DeltaTime*animator.Speed)%animData.Time;

                int newFrameIndex = (int)math.clamp(animator.Time/animData.Time*animData.Length,0,animData.Length-1);

                if(newFrameIndex!=animator.FrameIndex)
                {
                    // Debug.Log($"+{animData.Start}:{animator.FrameIndex}");
                    ECB.SetComponentEnabled<MaterialFlipbookAnimUpdate>(chunkInQueryIndex,entity,true);
                    ECB.SetComponent(chunkInQueryIndex,entity,new MaterialFlipbookAnimUpdate
                    {
                        // Start = animator.FrameIndex,
                        // Length = (int)(deltaTime/frameTime),
                        FlipbookIndex = animData.Start+newFrameIndex,
                        // FrameCount = animData.Length,
                    });
                    animator.FrameIndex = newFrameIndex;
                }
            }
        }
    }
}

