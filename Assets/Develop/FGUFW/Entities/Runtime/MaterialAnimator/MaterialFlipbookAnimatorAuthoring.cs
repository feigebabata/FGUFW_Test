using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Rendering;
using UnityEngine;
using Unity.Mathematics;
using System;
using Unity.Collections;
using UnityEngine.Rendering;

namespace FGUFW.Entities
{
    public partial class MaterialFlipbookAnimatorSystemGroup : ComponentSystemGroup{}

    public class MaterialFlipbookAnimatorAuthoring : MonoBehaviour
    {
        public float Speed=1;
        public float Start;
    }

    public class MaterialFlipbookAnimatorBaker : Baker<MaterialFlipbookAnimatorAuthoring>
    {
        public override void Bake(MaterialFlipbookAnimatorAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity,new MaterialFlipbookAnimator
            {
                Speed = authoring.Speed,
                Time = authoring.Start,
            });
        }
    }

    public struct MaterialFlipbookAnimator:IComponentData
    {
        public float Speed;//播放速度
        public float Time;//当前时间 每帧叠加detalTime 切换动画时需要置为0
        public int FrameIndex;//当前帧索引
        public int EventCount;
    }

    public struct MaterialFlipbookAnimationData:IComponentData
    {
        public BatchMaterialID MaterialID;
        public int Start;
        public int Length;
        public float Time;
        public bool Loop;
    }

}