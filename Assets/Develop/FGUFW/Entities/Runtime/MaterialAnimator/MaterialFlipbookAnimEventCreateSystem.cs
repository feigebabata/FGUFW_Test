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
    [BurstCompile]
    public partial struct MaterialFlipbookAnimEventCreateSystem : ISystem
    {

        private EntityQuery _eq;

        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            _eq = new EntityQueryBuilder(Allocator.Temp).WithAll<MaterialFlipbookAnimator>().Build(ref state);
            state.RequireForUpdate(_eq);

            var ecb = new EntityCommandBuffer(Allocator.Temp);
            ecb.AddComponent(ecb.CreateEntity(),new MaterialFlipbookAnimEventSingleton
            {
                Events = new NativeList<MaterialFlipbookAnimEventCastData>(64,Allocator.Persistent)
            });
            ecb.Playback(state.EntityManager);
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var singletonRW = SystemAPI.GetSingletonRW<MaterialFlipbookAnimEventSingleton>();

            var animators = _eq.ToComponentDataArray<MaterialFlipbookAnimator>(Allocator.Temp);
            int allEventCount = 0;
            foreach (var animator in animators)
            {
                allEventCount += animator.EventCount;
            }
            animators.Dispose();

            if(singletonRW.ValueRO.Events.Capacity<allEventCount)
            {
                singletonRW.ValueRW.Events.Dispose();
                singletonRW.ValueRW.Events = new NativeList<MaterialFlipbookAnimEventCastData>(allEventCount,Allocator.Persistent);
            }

            state.Dependency = new AnimEventJob
            {
                PW = singletonRW.ValueRW.Events.AsParallelWriter(),
            }
            .ScheduleParallel(state.Dependency);

            state.Dependency.Complete();
        }

        [BurstCompile]
        partial struct AnimEventJob:IJobEntity
        {
            public NativeList<MaterialFlipbookAnimEventCastData>.ParallelWriter PW;

            void Execute(Entity entity,in DynamicBuffer<MaterialFlipbookAnimEventData> events,ref MaterialFlipbookAnimator animator)
            {
                if(animator.FrameIndex==animator.PrevFrameIndex)return;
                if(animator.Speed==0)return;

                foreach (var eventData in events)
                {
                    bool eventCast = false;
                    if(animator.Speed>0)
                    {
                        if(animator.FrameIndex>animator.PrevFrameIndex)
                        {
                            if(eventData.FrameIndex>animator.PrevFrameIndex && eventData.FrameIndex<=animator.FrameIndex)
                            {
                                eventCast = true;
                            }
                        }
                        else
                        {
                            if(eventData.FrameIndex>animator.PrevFrameIndex || eventData.FrameIndex<=animator.FrameIndex)
                            {
                                eventCast = true;
                            }
                        }
                    }
                    else
                    {
                        if(animator.FrameIndex<animator.PrevFrameIndex)
                        {
                            if(eventData.FrameIndex<animator.PrevFrameIndex && eventData.FrameIndex>=animator.FrameIndex)
                            {
                                eventCast = true;
                            }
                        }
                        else
                        {
                            if(eventData.FrameIndex<animator.PrevFrameIndex || eventData.FrameIndex>=animator.FrameIndex)
                            {
                                eventCast = true;
                            }
                        }
                    }

                    if(eventCast)
                    {
                        Debug.LogWarning($"{animator.FrameIndex} {animator.PrevFrameIndex}");
                        PW.AddNoResize(new MaterialFlipbookAnimEventCastData
                        {
                            Self = entity,
                            Event = eventData.FrameEvent,
                        });
                    }
                    animator.PrevFrameIndex = animator.FrameIndex;
                }
            }

        }

    }
}

