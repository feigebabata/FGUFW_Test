using UnityEngine;
using Unity.Entities;
using Unity.Collections;
using Unity.Mathematics;
using static Unity.Entities.SystemAPI;
using static Unity.Entities.SystemAPI.ManagedAPI;
using Unity.Burst;

namespace FGUFW.ECS_SpriteAnimator
{
    [UpdateAfter(typeof(SpriteAnimaionPlaySystem))]
    [UpdateInGroup(typeof(SpriteAnimatorSystemGroup))]
    [BurstCompile]
    partial struct SpriteAnimationUpdateSystem:ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            EntityQuery eq = new EntityQueryBuilder(Allocator.Temp).WithAll<SpriteAnimUpdate,SpriteAnimFrameData>().Build(ref state);
            state.RequireForUpdate(eq);
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var ecb = SystemAPI.GetSingleton<EndFixedStepSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);
            foreach (var (spriteAnimUpdate,spriteAnimFrameData) in Query<SpriteAnimUpdate,SpriteAnimFrameData>())
            {
                GetComponent<SpriteRenderer>(spriteAnimUpdate.Self).sprite = spriteAnimFrameData.Frames[spriteAnimUpdate.FrameIndex];
                ecb.RemoveComponent<SpriteAnimUpdate>(spriteAnimUpdate.Self);
            }
        }
    }
}