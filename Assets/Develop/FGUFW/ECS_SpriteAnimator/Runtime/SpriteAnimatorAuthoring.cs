using UnityEngine;
using Unity.Entities;
using Unity.Collections;
using Unity.Mathematics;

namespace FGUFW.ECS_SpriteAnimator
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class SpriteAnimatorAuthoring : MonoBehaviour
    {
        
    }

    class SpriteAnimatorBaker : Baker<SpriteAnimatorAuthoring>
    {
        public override void Bake(SpriteAnimatorAuthoring authoring)
        {
            AddComponent(new SpriteAnimator
            {
                Self = GetEntity(authoring),
                Speed = 1
            });
        }
    }

    struct SpriteAnimator:IComponentData
    {
        public Entity Self;
        public float Speed;//播放速度
        public float Time;//当前时间 每帧叠加detalTime 切换动画时需要置为0
        public int FrameIndex;//当前帧索引
    }

    class SpriteAnimFrameData:IComponentData//动画帧 托管数据
    {
        public readonly Sprite[] Frames;
    }

    struct SpriteAnimInfoData:IComponentData
    {
        public readonly int ID;//动画ID
        public readonly float Length;//动画时长
        public readonly int FrameCount;//总帧数
    }


    struct SpriteAnimEventData:IComponentData
    {
        public readonly NativeArray<SpriteEvent> Events;//没事件就没有这个组件
    }

    struct SpriteEvent
    {
        public readonly int FrameIndex;//第几帧触发事件 如果超过动画帧就在动画帧末触发 并警告
        public readonly int FrameEvent;//事件id 手动转事件枚举/转事件组件
    }
}