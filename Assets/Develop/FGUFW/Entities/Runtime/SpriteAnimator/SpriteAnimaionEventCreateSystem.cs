using UnityEngine;
using Unity.Entities;
using Unity.Collections;
using Unity.Mathematics;
using Unity.Burst;
using static Unity.Entities.SystemAPI;
using System;
using Unity.Jobs;

namespace FGUFW.Entities
{
    [UpdateAfter(typeof(SpriteAnimaionPlaySystem))]
    [UpdateBefore(typeof(SpriteAnimationUpdateSystem))]
    [UpdateInGroup(typeof(SpriteAnimatorSystemGroup))]
    [BurstCompile]
    public partial struct SpriteAnimaionEventCreateSystem:ISystem
    {
        private EntityQuery _eq;


        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            _eq = new EntityQueryBuilder(Allocator.Temp).
            WithAll<SpriteAnimUpdate,SpriteAnimEventsData>().Build(ref state);
            state.RequireForUpdate(_eq);
            
            var ecb = new EntityCommandBuffer(Allocator.Temp);
            ecb.AddComponent(ecb.CreateEntity(),new SpriteAnimEventsSingleton
            {
                Events = new NativeList<SpriteAnimEventData>(64,Allocator.Persistent)
            });
            ecb.Playback(state.EntityManager);
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var singletonRW = SystemAPI.GetSingletonRW<SpriteAnimEventsSingleton>();

            var spriteAnimEventsDatas = _eq.ToComponentDataArray<SpriteAnimEventsData>(Allocator.Temp);
            int allEventCount = 0;
            foreach (var eventsData in spriteAnimEventsDatas)
            {
                allEventCount += eventsData.Events.Length;
            }
            spriteAnimEventsDatas.Dispose();
            if(singletonRW.ValueRO.Events.Capacity<allEventCount)
            {
                singletonRW.ValueRW.Events.Dispose();
                singletonRW.ValueRW.Events = new NativeList<SpriteAnimEventData>(allEventCount,Allocator.Persistent);
            }
            
            state.Dependency = new SpriteAnimaionEventCreateJob()
            {
                PW = singletonRW.ValueRW.Events.AsParallelWriter()
            }
            .ScheduleParallel(state.Dependency);
            state.Dependency.Complete();

            if(singletonRW.ValueRW.Events.Length>0)
            {
                // Debug.Log($"Create {singletonRW.ValueRW.Events.Length}");
            }
            
        }

        [BurstCompile]
        partial struct SpriteAnimaionEventCreateJob : IJobEntity
        {
            public NativeList<SpriteAnimEventData>.ParallelWriter PW;

            public void Execute(Entity entity,in SpriteAnimUpdate spriteAnimUpdate,in SpriteAnimEventsData spriteAnimEventsData)
            {
                foreach (var eventData in spriteAnimEventsData.Events)
                {
                    if(eventData.FrameIndex==spriteAnimUpdate.FrameIndex)
                    {
                        PW.AddNoResize(new SpriteAnimEventData
                        {
                            Event = eventData,
                            Self = entity
                        });
                    }
                }
            }
        }

    }
}
