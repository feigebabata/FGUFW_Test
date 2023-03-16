// using UnityEngine;
// using Unity.Entities;
// using Unity.Collections;
// using Unity.Mathematics;
// using Unity.Burst;
// using static Unity.Entities.SystemAPI;

// namespace FGUFW.ECS_SpriteAnimator
// {
//     [UpdateAfter(typeof(SpriteAnimaionPlaySystem))]
//     [UpdateBefore(typeof(SpriteAnimationUpdateSystem))]
//     [UpdateInGroup(typeof(SpriteAnimatorSystemGroup))]
//     partial struct SpriteAnimaionEventCreateSystem:ISystem
//     {
//         [BurstCompile]
//         public void OnCreate(ref SystemState state)
//         {
//             EntityQuery eq = new EntityQueryBuilder(Allocator.Temp).
//             WithAll<SpriteAnimUpdate,SpriteAnimEventData>().Build(ref state);
//             state.RequireForUpdate(eq);
//         }

//         [BurstCompile]
//         public void OnUpdate(ref SystemState state)
//         {
//             foreach (var (spriteAnimUpdate,spriteAnimEventData) in Query<SpriteAnimUpdate,SpriteAnimEventData>())
//             {
//                 foreach (var eventData in spriteAnimEventData.Events)
//                 {
//                     if(eventData.FrameIndex==spriteAnimUpdate.FrameIndex)
//                     {
                        
//                     }
//                 }
//             }
//         }

//     }
// }
