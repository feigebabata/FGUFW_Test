using UnityEngine;
using Unity.Entities;
using Unity.Collections;
using Unity.Mathematics;
using Unity.Collections.LowLevel.Unsafe;

namespace FGUFW.Entities
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class SpriteAnimationDataAuthoring : MonoBehaviour
    {
        public bool Loop;
        public float AnimTime;
        public Sprite[] Frames;
        public int AnimID;
        public SpriteEvent[] Events;
    }

    class SpriteAnimationDataBaker : Baker<SpriteAnimationDataAuthoring>
    {
        public override void Bake(SpriteAnimationDataAuthoring authoring)
        {
            if(authoring.Frames==null || authoring.Frames.Length==0)return;
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponentObject(entity,new SpriteAnimFrameData
            {
                Frames = authoring.Frames
            });
            AddComponent(entity,new SpriteAnimInfoData
            {
                ID = authoring.AnimID,
                Length = authoring.AnimTime,
                FrameCount = authoring.Frames.Length,
                Loop = authoring.Loop
            });
            if(authoring.Events==null || authoring.Events.Length==0)return;

            var events = new UnsafeList<SpriteEvent>(authoring.Events.Length,Allocator.Persistent);
            foreach (var item in authoring.Events)
            {
                events.Add(item);
            }
            AddComponent(entity,new SpriteAnimEventsData
            {
                Events = events
            });
        }
    }

    public class SpriteAnimFrameData:IComponentData//动画帧 托管数据
    {
        public Sprite[] Frames;
    }

    public struct SpriteAnimInfoData:IComponentData
    {
        public int ID;//动画ID

        public float Length;//动画时长
        
        public int FrameCount;//总帧数
        
        public bool Loop;
    }


    public struct SpriteAnimEventsData:IComponentData//用户自定义事件检测和后续处理
    {
        public UnsafeList<SpriteEvent> Events;//没事件就没有这个组件
    }

    [System.Serializable]
    public struct SpriteEvent
    {
        public int FrameIndex;//第几帧触发事件 如果超过动画帧就在动画帧末触发 并警告
        
        public int FrameEvent;//事件id 手动转事件枚举/转事件组件 0为空
    }
    
}