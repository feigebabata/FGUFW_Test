using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using Unity.Burst;
using Unity.Collections;
using static Unity.Entities.SystemAPI;
using Unity.Physics;
using Unity.Transforms;
using Unity.Physics.Systems;

public class ForceMovementAuthoring : MonoBehaviour
{
    public float Force;//动力 矫正当前速度的方向和大小
    public float MaxVelocity;//最大速度
}

class ForceMovementBaker : Baker<ForceMovementAuthoring>
{
    public override void Bake(ForceMovementAuthoring authoring)
    {
        AddComponent(new ForceMovement
        {
            Force = authoring.Force,
            MaxVelocity = authoring.MaxVelocity
        });
    }
}

public struct ForceMovement:IComponentData //依赖PhysicsVelocity
{
    public float Force;//动力 矫正当前速度的方向和大小
    public float MaxVelocity;//最大速度
    public float3 TargetPoint;//目标方向
}

[UpdateBefore(typeof(PhysicsSystemGroup))]
[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
[BurstCompile]
partial struct ForceMovementSystem:ISystem
{

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        state.Dependency = new ForceMovementJob
        {
            DeltaTime = SystemAPI.Time.DeltaTime
        }
        .Schedule/*Parallel*/(state.Dependency);
    }

    partial struct ForceMovementJob : IJobEntity
    {
        [ReadOnly]
        public float DeltaTime;

        void Execute(in ForceMovement forceMovement,ref PhysicsVelocity physicsVelocity,ref WorldTransform transform)
        {
            float space = math.distance(forceMovement.TargetPoint,transform.Position);
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

            float3 targetDirection = math.normalize(forceMovement.TargetPoint-transform.Position);
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
            if(moveSpace>math.distance(transform.Position+selfVelocity*DeltaTime,forceMovement.TargetPoint))
            {
                selfVelocity = float3.zero;
                transform.Position = forceMovement.TargetPoint;
            }
            physicsVelocity.Linear = selfVelocity;

        }
    }

}
