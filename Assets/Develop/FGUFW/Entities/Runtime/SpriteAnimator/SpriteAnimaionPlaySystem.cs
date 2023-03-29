using Unity.Entities;
using Unity.Collections;
using Unity.Mathematics;
using Unity.Transforms;
using static Unity.Entities.SystemAPI;
using static Unity.Entities.SystemAPI.ManagedAPI;
using Unity.Burst;

namespace FGUFW.Entities
{
    [UpdateBefore(typeof(TransformSystemGroup))]
    /// <summary>
    /// SpriteAnimator系统所在组
    /// </summary>
    public class SpriteAnimatorSystemGroup : ComponentSystemGroup{}

    
    [BurstCompile]
    [UpdateInGroup(typeof(SpriteAnimatorSystemGroup))]
    partial struct SpriteAnimaionPlaySystem:ISystem
    {
        
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            EntityQuery eq = new EntityQueryBuilder(Allocator.Temp).WithNone<SpriteAnimUpdate>().WithAll<SpriteAnimator,SpriteAnimInfoData>().Build(ref state);
            state.RequireForUpdate(eq);
        }
        
        
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var ecb = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);
            state.Dependency = new SpriteAnimaionPlayJob
            {
                ECBP = ecb.AsParallelWriter(),
                DeltaTime = Time.DeltaTime
            }
            .ScheduleParallel(state.Dependency);
        }
        
        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {
            
        }

        [BurstCompile]
        partial struct SpriteAnimaionPlayJob:IJobEntity
        {
            public EntityCommandBuffer.ParallelWriter ECBP;

            [ReadOnly]
            public float DeltaTime;

            void Execute([ChunkIndexInQuery] int chunkInQueryIndex,Entity entity,in SpriteAnimInfoData spriteAnimInfoData,ref SpriteAnimator spriteAnimator)
            {
                if(!spriteAnimInfoData.Loop)
                {
                    spriteAnimator.Time = math.clamp(DeltaTime*spriteAnimator.Speed+spriteAnimator.Time,0,spriteAnimInfoData.Length);
                }
                else
                {
                    spriteAnimator.Time = (DeltaTime*spriteAnimator.Speed+spriteAnimator.Time)%spriteAnimInfoData.Length;
                }

                int newFrameIndex = (int)math.clamp(spriteAnimator.Time/spriteAnimInfoData.Length*spriteAnimInfoData.FrameCount,0,spriteAnimInfoData.FrameCount-1);

                if(newFrameIndex!=spriteAnimator.FrameIndex)
                {
                    spriteAnimator.FrameIndex = newFrameIndex;
                    // UnityEngine.Debug.Log($"newFrameIndex {newFrameIndex}");
                    ECBP.AddComponent(chunkInQueryIndex,entity,new SpriteAnimUpdate
                    {
                        FrameIndex = newFrameIndex
                    });
                }
            }
        }
    }
}