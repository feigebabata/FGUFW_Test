using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Rendering;
using UnityEngine;
using Unity.Mathematics;

namespace FGUFW.Entities
{
    public class MaterialFlipAuthoring : MonoBehaviour
    {
        public float Value;
    }

    public class MaterialFlipBaker : Baker<MaterialFlipAuthoring>
    {
        public override void Bake(MaterialFlipAuthoring authoring)
        {
            AddComponent(GetEntity(TransformUsageFlags.Dynamic),new MaterialFlip
            {
                Value = authoring.Value
            });
        }
    }

    [MaterialProperty("_Flip")]
    public struct MaterialFlip:IComponentData
    {
        public float Value;
    }

}