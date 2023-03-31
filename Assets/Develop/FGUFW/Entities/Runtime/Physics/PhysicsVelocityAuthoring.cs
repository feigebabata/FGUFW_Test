using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Physics;
using Unity.Entities;
using Unity.Mathematics;

namespace FGUFW.Entities
{

    public class PhysicsVelocityAuthoring : MonoBehaviour
    {
        public float3 Linear;
        public float3 Angular;
    }

    class PhysicsVelocityBaker : Baker<PhysicsVelocityAuthoring>
    {
        public override void Bake(PhysicsVelocityAuthoring authoring)
        {
            AddComponent(GetEntity(TransformUsageFlags.Dynamic),new PhysicsVelocity
            {
                Linear = authoring.Linear,
                Angular = authoring.Angular,
            });
        }
    }
}
