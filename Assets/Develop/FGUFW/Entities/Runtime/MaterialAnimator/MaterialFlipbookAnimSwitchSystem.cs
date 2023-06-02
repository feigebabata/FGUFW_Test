using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Rendering;
using UnityEngine;
using Unity.Mathematics;
using Unity.Collections;
using UnityEngine.Rendering;
using Unity.Burst;

#pragma warning disable CS0282

namespace FGUFW.Entities
{
    [UpdateInGroup(typeof(MaterialFlipbookAnimatorSystemGroup))]
    [UpdateBefore(typeof(MaterialFlipbookAnimPlaySystem))]
    [UpdateAfter(typeof(MaterialBakerSystem))]
    [BurstCompile]
    public partial struct MaterialFlipbookAnimSwitchSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            EntityQuery eq = new EntityQueryBuilder(Allocator.Temp).WithAll<MaterialFlipbookAnimationSwitch>().Build(ref state);
            state.RequireForUpdate(eq);
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var ecb = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);
            state.Dependency = new AnimSwitchJob
            {
                ECBP = ecb.AsParallelWriter(),
            }
            .ScheduleParallel(state.Dependency);
            // state.Dependency.Complete();
        }

        partial struct AnimSwitchJob:IJobEntity
        {
            public EntityCommandBuffer.ParallelWriter ECBP;

            void Execute([ChunkIndexInQuery] int chunkInQueryIndex,MaterialFlipbookAnimatorAspect aspect,in MaterialFlipbookAnimationSwitch animSwitch,in Entity entity)
            {
                aspect.AnimationID = animSwitch.AnimationID;
                aspect.Loop = animSwitch.Loop;
                aspect.Speed = animSwitch.Speed;
                
                ECBP.SetComponentEnabled<MaterialFlipbookAnimationSwitch>(chunkInQueryIndex,entity,false);
            }
        }

    }

    public readonly partial struct MaterialFlipbookAnimatorAspect:IAspect
    {
        public readonly Entity Self;
        readonly RefRW<MaterialFlipbookAnimator> _animator;
        readonly RefRW<MaterialFlipbookAnimationData> _animation;
        readonly DynamicBuffer<MaterialFlipbookAnimations> _animations;
        // readonly DynamicBuffer<MaterialFlipbookAnimEventData> _events;
        readonly RefRW<MaterialMeshInfo> _mmInfo;
        readonly RefRW<MaterialFlipbookIndex> _mfIndex;

        public int AnimationID
        {
            get
            {
                return _animation.ValueRO.AnimationID;
            }
            set
            {
                foreach (var item in _animations)
                {
                    if(item.Anim.AnimationID == value)
                    {
                        _animation.ValueRW = item.Anim;

                        // _events.Clear();
                        // foreach (var eventData in item.Events)
                        // {
                        //     _events.Add(eventData);
                        // }

                        _animator.ValueRW.Time=0;
                        _animator.ValueRW.FrameIndex=-1;
                        // _animator.ValueRW.EventCount=_events.Length;

                        _mmInfo.ValueRW.MaterialID = item.Anim.MaterialID;
                        _mfIndex.ValueRW.Value = item.Anim.Start;
                        // Debug.LogWarning($"{item.Anim.MaterialID.value}:{item.Anim.Start}-{item.Anim.Start+item.Anim.Length}");
                        return;
                    }
                }
            }
        }

        public bool Loop
        {
            get
            {
                return _animator.ValueRW.Loop;
            }
            set
            {
                _animator.ValueRW.Loop = value;
            }
        }

        public float Speed
        {
            get
            {
                return _animator.ValueRW.Speed;
            }
            set
            {
                _animator.ValueRW.Speed = value;
            }
        }

    }

}