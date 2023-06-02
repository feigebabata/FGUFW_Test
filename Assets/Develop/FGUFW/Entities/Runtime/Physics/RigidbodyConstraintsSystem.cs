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
#pragma warning disable CS0282

namespace FGUFW.Entities
{

    [UpdateBefore(typeof(PhysicsSystemGroup))]
    [UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
    [BurstCompile]
    partial struct RigidbodyConstraintsSystem:ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            EntityQuery eq = new EntityQueryBuilder(Allocator.Temp).WithAll<RigidbodyConstraints,PhysicsMass,PhysicsVelocity>().Build(ref state);
            state.RequireForUpdate(eq);
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var ecb = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);
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
                var angular = rigidbodyConstraintsAspect.Velocity.ValueRO.Angular;
                if(rigidbodyConstraintsAspect.Constraints.ValueRO.FreezeRotation.x)
                {
                    inverseInertia.x=0;
                    angular.x=0;
                }
                if(rigidbodyConstraintsAspect.Constraints.ValueRO.FreezeRotation.y)
                {
                    inverseInertia.y=0;
                    angular.y=0;
                }
                if(rigidbodyConstraintsAspect.Constraints.ValueRO.FreezeRotation.z)
                {
                    inverseInertia.z=0;
                    angular.z=0;
                }
                
                rigidbodyConstraintsAspect.Mass.ValueRW.InverseInertia = inverseInertia;
                rigidbodyConstraintsAspect.Velocity.ValueRW.Angular =angular;
                ECBP.RemoveComponent<RigidbodyConstraints>(chunkInQueryIndex,rigidbodyConstraintsAspect.Self);
            }
        }

    }

}

