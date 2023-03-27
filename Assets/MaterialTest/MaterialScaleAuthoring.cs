using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Rendering;
using UnityEngine;

public class MaterialScaleAuthoring : MonoBehaviour
{
    public float Value;
}

public class MaterialScaleBaker : Baker<MaterialScaleAuthoring>
{
    public override void Bake(MaterialScaleAuthoring authoring)
    {
        AddComponent(new MaterialScale
        {
            Value = authoring.Value
        });
    }
}

[MaterialProperty("_Scale")]
public struct MaterialScale:IComponentData
{
    public float Value;
}
