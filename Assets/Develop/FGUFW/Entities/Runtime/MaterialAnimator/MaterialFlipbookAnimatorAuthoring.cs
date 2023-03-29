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
    public class MaterialFlipbookAnimatorAuthoring : MonoBehaviour
    {
        public float Speed=1;
        public AnimationDatas Anims;

        [Serializable]
        public class AnimationDatas : IComponentData
        {
            public AnimationData[] Anims;
        }
        
        [Serializable]
        public class AnimationData
        {
            public Material Mat;
            public int Start;
            public int Length;
            public float Time;
        }
    }


    public class MaterialFlipbookAnimatorBaker : Baker<MaterialFlipbookAnimatorAuthoring>
    {
        public override void Bake(MaterialFlipbookAnimatorAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity,new MaterialFlipbookAnimator
            {
                
            });
            AddComponentObject(entity,authoring.Anims);
            AddBuffer<MaterialFlipbookAnimationData>(entity);
        }
    }

    public struct MaterialFlipbookAnimator:IComponentData
    {
        public float Speed;
    }

    public struct MaterialFlipbookAnimationData:IBufferElementData
    {
        public BatchMaterialID MaterialID;
        public int Start;
        public int Length;
        public float Time;
    }

}