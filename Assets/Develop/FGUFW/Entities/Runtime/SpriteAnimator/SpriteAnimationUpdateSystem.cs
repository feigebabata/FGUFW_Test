// using UnityEngine;
// using Unity.Entities;
// using Unity.Collections;
// using Unity.Mathematics;
// using static Unity.Entities.SystemAPI;
// using static Unity.Entities.SystemAPI.ManagedAPI;
// using Unity.Burst;

// namespace FGUFW.Entities
// {
//     [UpdateAfter(typeof(SpriteAnimaionPlaySystem))]
//     [UpdateInGroup(typeof(SpriteAnimatorSystemGroup))]
//     partial struct SpriteAnimationUpdateSystem:ISystem
//     {
//         public void OnCreate(ref SystemState state)
//         {
//             EntityQuery eq = new EntityQueryBuilder(Allocator.Temp).WithAll<SpriteAnimUpdate,SpriteAnimFrameData>().Build(ref state);
//             state.RequireForUpdate(eq);
//         }

//         public void OnUpdate(ref SystemState state)
//         {
//             var ecb = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);
//             foreach (var (spriteAnimUpdate,spriteAnimFrameData,entity) in Query<SpriteAnimUpdate,SpriteAnimFrameData>().WithEntityAccess())
//             {
//                 // Debug.Log($"update {spriteAnimUpdate.FrameIndex}");
//                 GetComponent<SpriteRenderer>(entity).sprite = spriteAnimFrameData.Frames[spriteAnimUpdate.FrameIndex];
//                 ecb.RemoveComponent<SpriteAnimUpdate>(entity);
//             }
//         }
//     }
// }