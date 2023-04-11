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

    [UpdateBefore(typeof(PhysicsSystemGroup))]
    [UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
    [BurstCompile]
    public partial struct ForceMovementSystem:ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            EntityQuery eq = new EntityQueryBuilder(Allocator.Temp).WithAll<ForceMovement,PhysicsVelocity,LocalTransform>().Build(ref state);
            state.RequireForUpdate(eq);
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            state.Dependency = new ForceMovementJob
            {
                DeltaTime = SystemAPI.Time.DeltaTime
            }
            .ScheduleParallel(state.Dependency);
        }

        [BurstCompile]
        partial struct ForceMovementJob : IJobEntity
        {
            [ReadOnly]
            public float DeltaTime;

            void Execute(in ForceMovement forceMovement,in ForceMovementTarget forceMovementTarget,ref PhysicsVelocity physicsVelocity,ref LocalTransform transform)
            {
                float space = math.distance(forceMovementTarget.TargetPoint,transform.Position);
                //已到达目的地则退出
                if(space==0)
                {
                    if(!math.all(physicsVelocity.Linear==float3.zero))
                    {
                        physicsVelocity.Linear=float3.zero;
                    }
                    return;
                }

                float3 selfVelocity = physicsVelocity.Linear;

                float3 targetDirection = math.normalize(forceMovementTarget.TargetPoint-transform.Position);
                // float3 selfDirection = math.normalize(selfVelocity);

                //理想速度
                float3 idealVelocity = targetDirection*forceMovement.MaxVelocity;
                if(!math.all(selfVelocity==idealVelocity))
                {
                    //理想纠正速度
                    float3 correctVelocity = idealVelocity - selfVelocity;

                    //最大纠正速率
                    float forceValue = forceMovement.Force*DeltaTime;

                    if(forceValue<math.length(correctVelocity))
                    {
                        correctVelocity = math.normalize(correctVelocity)*forceValue;
                    }

                    selfVelocity += correctVelocity;
                }

                float moveSpace = math.length(selfVelocity*DeltaTime);

                //如果当前帧移动会越过目标的 则代替物理系统位移
                if(moveSpace>math.distance(transform.Position+selfVelocity*DeltaTime,forceMovementTarget.TargetPoint))
                {
                    selfVelocity = float3.zero;
                    transform.Position = forceMovementTarget.TargetPoint;
                }
                physicsVelocity.Linear = selfVelocity;

            }
        }

    }
    
}
