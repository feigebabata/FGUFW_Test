// using UnityEngine;
// using Unity.Entities;
// using Unity.Collections;
// using Unity.Mathematics;

// namespace FGUFW.Entities
// {
//     [RequireComponent(typeof(SpriteRenderer))]
//     public class SpriteAnimatorAuthoring : MonoBehaviour
//     {
//         public float Speed=1;
//         public float StartTime=0;
//     }

//     class SpriteAnimatorBaker : Baker<SpriteAnimatorAuthoring>
//     {
//         public override void Bake(SpriteAnimatorAuthoring authoring)
//         {
//             AddComponent(GetEntity(TransformUsageFlags.Dynamic),new SpriteAnimator
//             {
//                 Speed = authoring.Speed,
//                 Time = authoring.StartTime
//             });
//         }
//     }

//     public struct SpriteAnimator:IComponentData
//     {
//         public float Speed;//播放速度
//         public float Time;//当前时间 每帧叠加detalTime 切换动画时需要置为0
//         public int FrameIndex;//当前帧索引
//     }

//     public struct SpriteAnimUpdate:IComponentData//动画帧更新事件
//     {
//         public int FrameIndex;
//     }
// }