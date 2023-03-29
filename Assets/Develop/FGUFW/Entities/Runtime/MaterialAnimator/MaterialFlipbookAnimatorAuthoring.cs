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
        public AnimationData[] Anims;
        
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
            AddComponent(new MaterialFlipbookAnimator
            {
                
            });
        }
    }

    public struct MaterialFlipbookAnimator:IComponentData
    {
        
    }

    public struct MaterialFlipbookAnimationData:IComponentData
    {
        public float Value;
    }

    public struct FlipbookMaterialsSingleton:IComponentData
    {
        public NativeHashMap<int,BatchMaterialID> MaterialMap;
    }

}