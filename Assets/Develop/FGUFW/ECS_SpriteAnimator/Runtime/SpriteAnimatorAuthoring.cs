using UnityEngine;
using Unity.Entities;
using Unity.Collections;
using Unity.Mathematics;

namespace FGUFW.ECS_SpriteAnimator
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class SpriteAnimatorAuthoring : MonoBehaviour
    {
        public float Speed=1;
    }

    class SpriteAnimatorBaker : Baker<SpriteAnimatorAuthoring>
    {
        public override void Bake(SpriteAnimatorAuthoring authoring)
        {
            AddComponent(new SpriteAnimator
            {
                Self = GetEntity(authoring),
                Speed = authoring.Speed,
            });
        }
    }

    public struct SpriteAnimator:IComponentData
    {
        public Entity Self;
        public float Speed;//播放速度
        public float Time;//当前时间 每帧叠加detalTime 切换动画时需要置为0
        public int FrameIndex;//当前帧索引
    }

    public struct SpriteAnimUpdate:IComponentData//动画帧更新事件
    {
        public Entity Self;
        public int FrameIndex;
    }
}