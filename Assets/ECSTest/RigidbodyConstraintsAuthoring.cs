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
    public readonly RefRO<RigidbodyConstraints> Constraints;
}

[UpdateInGroup(typeof(PhysicsSystemGroup))]
[UpdateBefore(typeof(PhysicsSimulationGroup))]
[BurstCompile]
partial struct RigidbodyConstraintsSystem:ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var ecb = SystemAPI.GetSingleton<EndFixedStepSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);
        state.Dependency = new RigidbodyConstraintsJob
        {
            ECBP = ecb.AsParallelWriter()
        }
        .ScheduleParallel(state.Dependency);
    }

    [BurstCompile]
    partial struct RigidbodyConstraintsJob:IJobEntity
    {
        public EntityCommandBuffer.ParallelWriter ECBP;

        void Execute([ChunkIndexInQuery] int chunkInQueryIndex,RigidbodyConstraintsAspect rigidbodyConstraintsAspect)
        {
            var inverseInertia = rigidbodyConstraintsAspect.Mass.ValueRO.InverseInertia;
            if(rigidbodyConstraintsAspect.Constraints.ValueRO.FreezeRotation.x)inverseInertia.x=0;
            if(rigidbodyConstraintsAspect.Constraints.ValueRO.FreezeRotation.y)inverseInertia.y=0;
            if(rigidbodyConstraintsAspect.Constraints.ValueRO.FreezeRotation.z)inverseInertia.z=0;
            
            rigidbodyConstraintsAspect.Mass.ValueRW.InverseInertia = inverseInertia;
            ECBP.RemoveComponent<RigidbodyConstraints>(chunkInQueryIndex,rigidbodyConstraintsAspect.Self);
        }
    }

}