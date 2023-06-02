using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Rendering;
using UnityEngine;
using Unity.Mathematics;

namespace FGUFW.Entities
{
    [AddComponentMenu("MaterialProperty/Size")]
    [DisallowMultipleComponent]
    public class MaterialSizeAuthoring : MonoBehaviour
    {
        public float2 Value = new float2(1,1);
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