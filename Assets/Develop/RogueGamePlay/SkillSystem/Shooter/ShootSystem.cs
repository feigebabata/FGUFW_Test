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
using FGUFW;

namespace RogueGamePlay
{
    [BurstCompile]
    [UpdateAfter(typeof(PlayerShootControlSystem))]
    partial struct ShootSystem : ISystem
    {
        ComponentLookup<LocalTransform> _localTransforms;
        BufferLookup<Shooter> _shooters;

        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            EntityQuery eq = new EntityQueryBuilder(Allocator.Temp).WithAll<Shooter>().Build(ref state);
            state.RequireForUpdate(eq);

            _localTransforms = state.GetComponentLookup<LocalTransform>(true);
            _shooters = state.GetBufferLookup<Shooter>(false);
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            _localTransforms.Update(ref state);
            _shooters.Update(ref state);

            var ecb = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);
            var playerShootSingleton = SystemAPI.GetSingleton<ShootDirectionSingleton>();

            state.Dependency = new ShootEventJob
            {
                ECB = ecb,
                ShootDirection = playerShootSingleton,
                Shooters = _shooters,
                LocalTransforms = _localTransforms,
                Time = (float)SystemAPI.Time.ElapsedTime,
            }
            .Schedule(SystemAPI.GetSingleton<SkillEventSingleton>(),state.Dependency);
        }


        [BurstCompile]
        partial struct ShootEventJob : ISkillEventJob
        {
            public EntityCommandBuffer ECB;

            [ReadOnly]
            public ShootDirectionSingleton ShootDirection;

            // [NativeDisableUnsafePtrRestriction]
            public BufferLookup<Shooter> Shooters;

            [ReadOnly]
            public ComponentLookup<LocalTransform> LocalTransforms;

            public float Time;

            public void Execute(SkillEventData eventData)
            {
                var entity = eventData.Origin;
                if(!Shooters.HasBuffer(entity))return;

                var startPoint = eventData.Position;
                var shooters = Shooters[entity];
                var localTransform = LocalTransforms[entity];

                for (int i = 0; i < shooters.Length; i++)
                {
                    var shooter = shooters[i];
                    if(new BitEnums<SkillEvent>((uint)shooter.Triggers)[(uint)eventData.Event])
                    {
                        if(shooter.LastShootTime+shooter.IntervalTime<Time)
                        {
                            var airPos = ShootDirection.Points[(int)shooter.Direction];
                            var dir = new float3(1,0,0);
                            if(!math.all(airPos==startPoint))
                            {
                                dir = math.normalize(airPos-startPoint);
                            }

                            var shootCount = shooter.ShootCount;
                            var shootAngle = shooter.ShootAngle;
                            
                            for (int j = 0; j < shootCount; j++)
                            {
                                var d = VectorHelper.ShootAngle(dir,new float3(0,0,1),shootAngle,shootCount,j);
                                var attackerE = ECB.Instantiate(shooter.Attacker);
                                ECB.SetComponent(attackerE,new ForceMovementTarget
                                {
                                    TargetPoint = d*10000,
                                });
                                
                                ECB.SetComponent(attackerE,localTransform);
                                ECB.SetComponent(attackerE,new StartTime
                                {
                                    Time = Time,
                                });

                            }
                            shooter.LastShootTime = Time;
                            shooters[i] = shooter;
                        }
                    }
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
