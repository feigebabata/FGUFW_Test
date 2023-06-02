using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Rendering;
using UnityEngine;
using Unity.Mathematics;

namespace FGUFW.Entities
{
    [AddComponentMenu("MaterialProperty/Flip")]
    [DisallowMultipleComponent]
    public class MaterialFlipAuthoring : MonoBehaviour
    {
        public float Value=1;
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
        /// <summary>
        /// 1:镜像,-1:不镜像
        /// </summary>
        public float Value;
    }

}