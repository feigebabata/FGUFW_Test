using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Rendering;
using UnityEngine;
using Unity.Mathematics;

namespace FGUFW.Entities
{
    [AddComponentMenu("MaterialProperty/FlipbookIndex")]
    [DisallowMultipleComponent]
    public class MaterialFlipbookIndexAuthoring : MonoBehaviour
    {
        public float Value;
    }

    public class MaterialFlipbookIndexBaker : Baker<MaterialFlipbookIndexAuthoring>
    {
        public override void Bake(MaterialFlipbookIndexAuthoring authoring)
        {
            AddComponent(GetEntity(TransformUsageFlags.Dynamic),new MaterialFlipbookIndex
            {
                Value = authoring.Value
            });
        }
    }

    [MaterialProperty("_FlipbookIndex")]
    public struct MaterialFlipbookIndex:IComponentData
    {
        public float Value;
    }

}