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
    public class MaterialFlipbookAnimation : MonoBehaviour,IComponentData
    {
        public Material Mat;
        public int StartFrame;
        public int FrameLength;
        public float Time;
        public bool Loop;
    }

    class MaterialFlipbookAnimationBaker : Baker<MaterialFlipbookAnimation>
    {
        public override void Bake(MaterialFlipbookAnimation component)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponentObject(entity,component);
        }
    }


}