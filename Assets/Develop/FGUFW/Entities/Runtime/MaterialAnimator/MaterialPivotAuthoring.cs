using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Rendering;
using UnityEngine;
using Unity.Mathematics;

namespace FGUFW.Entities
{
    public class MaterialPivotAuthoring : MonoBehaviour
    {
        public float2 Value;
    }

    public class MaterialPivotBaker : Baker<MaterialPivotAuthoring>
    {
        public override void Bake(MaterialPivotAuthoring authoring)
        {
            AddComponent(GetEntity(TransformUsageFlags.Dynamic),new MaterialPivot
            {
                Value = authoring.Value
            });
        }
    }

    [MaterialProperty("_Pivot")]
    public struct MaterialPivot:IComponentData
    {
        public float2 Value;
    }

}