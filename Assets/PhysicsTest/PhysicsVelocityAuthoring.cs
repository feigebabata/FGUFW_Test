using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Physics;
using Unity.Entities;
using Unity.Mathematics;

public class PhysicsVelocityAuthoring : MonoBehaviour
{
    public float3 Linear;
    public float3 Angular;
}

class PhysicsVelocityBaker : Baker<PhysicsVelocityAuthoring>
{
    public override void Bake(PhysicsVelocityAuthoring authoring)
    {
        AddComponent(new PhysicsVelocity
        {
            Linear = authoring.Linear,
            Angular = authoring.Angular,
        });
    }
}
