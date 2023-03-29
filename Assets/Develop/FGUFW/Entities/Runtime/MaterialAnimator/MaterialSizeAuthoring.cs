using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Rendering;
using UnityEngine;
using Unity.Mathematics;

namespace FGUFW.Entities
{
    public class MaterialSizeAuthoring : MonoBehaviour
    {
        public float2 Value;
    }

    public class MaterialSizeBaker : Baker<MaterialSizeAuthoring>
    {
        public override void Bake(MaterialSizeAuthoring authoring)
        {
            AddComponent(GetEntity(TransformUsageFlags.Dynamic),new MaterialSize
            {
                Value = authoring.Value
            });
        }
    }

    [MaterialProperty("_Size")]
    public struct MaterialSize:IComponentData
    {
        public float2 Value;
    }

}