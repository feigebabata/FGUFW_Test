using UnityEngine;
using Unity.Entities;
using Unity.Collections;
using Unity.Mathematics;
using Unity.Transforms;
using static Unity.Entities.SystemAPI;
using static Unity.Entities.SystemAPI.ManagedAPI;

namespace FGUFW.ECS_SpriteAnimator
{
    [UpdateBefore(typeof(TransformSystemGroup))]
    /// <summary>
    /// SpriteAnimator系统所在组
    /// </summary>
    public class SpriteAnimatorSystemGroup : ComponentSystemGroup{}

    [UpdateInGroup(typeof(SpriteAnimatorSystemGroup))]
    partial struct SpriteAnimaionPlaySystem:ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            EntityQuery eq = new EntityQueryBuilder(Allocator.Temp).WithAll<SpriteAnimator,SpriteAnimInfoData,SpriteAnimFrameData,SpriteRenderer>().Build(ref state);
            state.RequireForUpdate(eq);
        }
        
        public void OnUpdate(ref SystemState state)
        {
            foreach (var (spriteAnimator,spriteAnimInfoData) in Query<RefRW<SpriteAnimator>,SpriteAnimInfoData>())
            {
                spriteAnimator.ValueRW.Time = (SystemAPI.Time.DeltaTime+spriteAnimator.ValueRW.Time)%spriteAnimInfoData.Length;

                var newFrameIndex = (int)(spriteAnimator.ValueRW.Time/spriteAnimInfoData.Length);

                if(newFrameIndex==spriteAnimator.ValueRW.FrameIndex)continue;

                spriteAnimator.ValueRW.FrameIndex = newFrameIndex;
                var entity = spriteAnimator.ValueRW.Self;
                GetComponent<SpriteRenderer>(entity).sprite = GetComponent<SpriteAnimFrameData>(entity).Frames[newFrameIndex];
            }
        }
        
        public void OnDestroy(ref SystemState state)
        {

        }
    }
}