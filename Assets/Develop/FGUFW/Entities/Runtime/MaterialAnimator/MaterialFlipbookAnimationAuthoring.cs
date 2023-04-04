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
    public class MaterialFlipbookAnimationAuthoring : MonoBehaviour
    {
        public MaterialFlipbookAnimation Animation;
    }

    [Serializable]
    public class MaterialFlipbookAnimation:IComponentData
    {
        public int AnimationID;
        public Material Mat;
        public int StartFrame;
        public int FrameLength;
        public float Time;
        public bool Loop;
    }

    class MaterialFlipbookAnimationBaker : Baker<MaterialFlipbookAnimationAuthoring>
    {
        public override void Bake(MaterialFlipbookAnimationAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponentObject(entity,authoring.Animation);
        }
    }


}