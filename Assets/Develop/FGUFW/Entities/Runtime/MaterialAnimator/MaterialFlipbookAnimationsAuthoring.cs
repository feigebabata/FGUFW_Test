using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Rendering;
using UnityEngine;
using Unity.Mathematics;
using System;
using Unity.Collections;
using UnityEngine.Rendering;
using Unity.Collections.LowLevel.Unsafe;

namespace FGUFW.Entities
{
    [AddComponentMenu("MaterialFlipbookAnim/Animations")]
    public class MaterialFlipbookAnimationsAuthoring : MonoBehaviour
    {
        public MaterialFlipbookAnimationsBakerData.Animation[] Animations;

    }

    public class MaterialFlipbookAnimationsBakerData:IComponentData
    {        
        public Animation[] Animations;

        [System.Serializable]
        public class Animation
        {
            public MaterialFlipbookAnimation Anim;
            public MaterialFlipbookAnimEventData[] Events;
        }  
    }

    public struct MaterialFlipbookAnimations:IBufferElementData
    {
        public MaterialFlipbookAnimationData Anim;
        public UnsafeList<MaterialFlipbookAnimEventData> Events;
    }

    public struct MaterialFlipbookAnimationSwitch:IComponentData
    {
        public int AnimationID;
    }


    class MaterialFlipbookAnimationsBaker : Baker<MaterialFlipbookAnimationsAuthoring>
    {
        public override void Bake(MaterialFlipbookAnimationsAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponentObject(entity,new MaterialFlipbookAnimationsBakerData
            {
                Animations = authoring.Animations,
            });
        }
    }


}