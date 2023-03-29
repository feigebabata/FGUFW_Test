using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Rendering;
using UnityEngine;
using Unity.Mathematics;

namespace FGUFW.Entities
{
    public class MaterialFlipbookSizeAuthoring : MonoBehaviour
    {
        public float2 Value;
    }

    public class MaterialFlipbookSizeBaker : Baker<MaterialFlipbookSizeAuthoring>
    {
        public override void Bake(MaterialFlipbookSizeAuthoring authoring)
        {
            AddComponent(GetEntity(TransformUsageFlags.Dynamic),new MaterialFlipbookSize
            {
                Value = authoring.Value
            });
        }
    }

    [MaterialProperty("_FlipbookSize")]
    public struct MaterialFlipbookSize:IComponentData
    {
        public float2 Value;
    }

}