using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using Unity.Burst;
using Unity.Collections;
using Unity.Physics;
using Unity.Transforms;
using Unity.Physics.Systems;

namespace FGUFW.Entities
{
    public class RigidbodyConstraintsAuthoring : MonoBehaviour
    {
        public bool3 FreezeRotation;
    }

    class RigidbodyConstraintsBaker : Baker<RigidbodyConstraintsAuthoring>
    {
        public override void Bake(RigidbodyConstraintsAuthoring authoring)
        {
            AddComponent(new RigidbodyConstraints
            {
                FreezeRotation = authoring.FreezeRotation
            });
        }
    }

    public struct RigidbodyConstraints:IComponentData 
    {
        public bool3 FreezeRotation;
    }

    public readonly partial struct RigidbodyConstraintsAspect:IAspect
    {
        public readonly Entity Self;
        public readonly RefRW<PhysicsMass> Mass;
        public readonly RefRW<PhysicsVelocity> Velocity;
        public readonly RefRO<RigidbodyConstraints> Constraints;
    }



}

