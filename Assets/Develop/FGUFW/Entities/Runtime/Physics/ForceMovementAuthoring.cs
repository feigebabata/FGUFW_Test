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
    
    [DisallowMultipleComponent]
    public class ForceMovementAuthoring : MonoBehaviour
    {
        public float Force;//动力 矫正当前速度的方向和大小
        public float MaxVelocity;//最大速度
    }

    class ForceMovementBaker : Baker<ForceMovementAuthoring>
    {
        public override void Bake(ForceMovementAuthoring authoring)
        {
            var entity = GetEntity(authoring,TransformUsageFlags.Dynamic);
            AddComponent(entity,new ForceMovement
            {
                Force = authoring.Force,
                MaxVelocity = authoring.MaxVelocity
            });
            AddComponent(entity,new ForceMovementTarget
            {
            });
        }
    }

    public struct ForceMovement:IComponentData //依赖PhysicsVelocity
    {
        public float Force;//动力 矫正当前速度的方向和大小
        public float MaxVelocity;//最大速度
    }

    public struct ForceMovementTarget:IComponentData
    {
        public float3 TargetPoint;//目标方向
    }

    
}
