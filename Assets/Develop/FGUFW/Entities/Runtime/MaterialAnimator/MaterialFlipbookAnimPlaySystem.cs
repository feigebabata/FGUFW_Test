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
    [UpdateAfter(typeof(MaterialBakerSystem))]
    [BurstCompile]
    partial struct MaterialFlipbookAnimPlaySystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            EntityQuery eq = new EntityQueryBuilder(Allocator.Temp).WithAll<MaterialFlipbookAnimator,MaterialFlipbookAnimationData,MaterialFlipbookIndex>().Build(ref state);
            state.RequireForUpdate(eq);
        }


        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            if(SystemAPI.Time.DeltaTime==0)return;

            state.Dependency = new AnimPlayJob
            {
                DeltaTime = SystemAPI.Time.DeltaTime,
            }
            .ScheduleParallel(state.Dependency);
        }

        [BurstCompile]
        partial struct AnimPlayJob:IJobEntity
        {
            [ReadOnly]public float DeltaTime;

            void Execute(in MaterialFlipbookAnimationData animData,ref MaterialFlipbookAnimator animator,ref MaterialFlipbookIndex materialIndex)
            {
                if(animator.Speed==0)return;
                if(!animData.Loop && animator.FrameIndex>=animData.Length-1)return;
                
                animator.Time = (animator.Time + DeltaTime*animator.Speed)%animData.Time;

                int newFrameIndex = (int)math.clamp(animator.Time/animData.Time*animData.Length,0,animData.Length-1);

                if(newFrameIndex!=animator.FrameIndex)
                {
                    animator.PrevFrameIndex = animator.FrameIndex;
                    animator.FrameIndex = newFrameIndex;
                    materialIndex.Value = animData.Start+newFrameIndex;
                }
            }
        }
    }
}

