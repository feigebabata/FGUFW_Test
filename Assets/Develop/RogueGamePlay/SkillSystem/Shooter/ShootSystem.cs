using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Collections;
using Unity.Burst;
using Unity.Transforms;
using Unity.Physics;
using FGUFW.Entities;
using Unity.Collections.LowLevel.Unsafe;

namespace RogueGamePlay
{
    [BurstCompile]
    [UpdateAfter(typeof(PlayerShootControlSystem))]
    partial struct ShootSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            EntityQuery eq = new EntityQueryBuilder(Allocator.Temp).WithAll<Shooter>().Build(ref state);
            state.RequireForUpdate(eq);
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var playerE = SystemAPI.GetSingletonEntity<Player>();
            var playerShooterRW = SystemAPI.GetComponentRW<Shooter>(playerE,false);
            var playerLT = SystemAPI.GetComponent<LocalTransform>(playerE);
            var ecb = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);
            var playerShootSingleton = SystemAPI.GetSingleton<PlayerShootSingleton>();

            state.Dependency = new ShootEventJob
            {
                ECB = ecb,
                PlayerShooter = playerShooterRW,
                PlayerShootSingleton = playerShootSingleton,
                PlayerLT = playerLT,
                Time = (float)SystemAPI.Time.ElapsedTime,
            }
            .Schedule(SystemAPI.GetSingleton<SkillEventSingleton>(),state.Dependency);
        }


        [BurstCompile]
        partial struct ShootEventJob : ISkillEventJob
        {
            public EntityCommandBuffer ECB;

            [ReadOnly]
            public PlayerShootSingleton PlayerShootSingleton;

            [NativeDisableUnsafePtrRestriction]
            public RefRW<Shooter> PlayerShooter;

            public LocalTransform PlayerLT;

            public float Time;

            public void Execute(SkillEventData eventData)
            {
                switch (eventData.Event)
                {
                    case SkillEvent.Shoot:
                    {
                        if(PlayerShooter.ValueRO.LastShootTime+PlayerShooter.ValueRO.IntervalTime<Time)
                        {
                            var attackerE = ECB.Instantiate(PlayerShooter.ValueRO.Attacker);
                            var airPos = PlayerShootSingleton.Points[(int)PlayerShooter.ValueRO.Direction];
                            var dir = new float3(1,0,0);
                            if(!math.all(airPos==PlayerLT.Position))
                            {
                                dir = math.normalize(airPos-PlayerLT.Position);
                            }
                            
                            ECB.SetComponent(attackerE,new ForceMovementTarget
                            {
                                TargetPoint = dir*10000,
                            });
                            ECB.SetComponent(attackerE,PlayerLT);

                            PlayerShooter.ValueRW.LastShootTime = Time;
                        }
                    }
                    break;
                }
            }
        }

        [BurstCompile]
        partial struct AutoShootJob:IJobEntity
        {
            public EntityCommandBuffer.ParallelWriter ECB;

            void Execute([ChunkIndexInQuery] int chunkInQueryIndex)
            {
                
            }
        }


    }
}
