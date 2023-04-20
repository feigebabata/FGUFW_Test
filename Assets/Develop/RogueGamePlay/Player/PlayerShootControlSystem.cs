using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Collections;
using Unity.Burst;
using Unity.Transforms;

namespace RogueGamePlay
{
    public struct ShootDirectionSingleton:IComponentData
    {
        public NativeArray<float3> Points;
    }

    [BurstCompile]
    [UpdateAfter(typeof(SkillEventDestroySystem))]
    partial struct PlayerShootControlSystem : ISystem
    {
        EntityQuery _eqMonster;

        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            EntityQuery eq = new EntityQueryBuilder(Allocator.Temp).WithAll<Shooter>().Build(ref state);
            state.RequireForUpdate(eq);

            _eqMonster = new EntityQueryBuilder(Allocator.Temp).WithAll<Monster,LocalTransform>().Build(ref state);

            var ecb = new EntityCommandBuffer(Allocator.Temp);
            ecb.AddComponent(ecb.CreateEntity(),new ShootDirectionSingleton
            {
                Points = new NativeArray<float3>((int)ShootDirectionType.Count,Allocator.Persistent),
            });
            ecb.Playback(state.EntityManager);
        }

        public void OnUpdate(ref SystemState state)
        {
            var playerShootSingleton = SystemAPI.GetSingletonRW<ShootDirectionSingleton>();
            var playerE = SystemAPI.GetSingletonEntity<Player>();
            var playerPos = SystemAPI.GetComponent<LocalTransform>(playerE).Position;

            var localTransforms = _eqMonster.ToComponentDataArray<LocalTransform>(Allocator.Temp);
            var monsterNear = playerPos;
            if(localTransforms.IsCreated && localTransforms.Length>0)
            {
                monsterNear = localTransforms[0].Position;
                float space = math.distance(monsterNear,playerPos);
                for (int i = 1; i < localTransforms.Length; i++)
                {
                    var monsterPos = localTransforms[i].Position;
                    if(math.distance(monsterPos,playerPos)<space)
                    {
                        monsterNear = monsterPos;
                        space = math.distance(monsterPos,playerPos);
                    }
                }
            }

            playerShootSingleton.ValueRW.Points[(int)ShootDirectionType.Player] = playerPos;
            playerShootSingleton.ValueRW.Points[(int)ShootDirectionType.MonsterNear] = monsterNear;

            if(Input.GetMouseButton(0))
            {
                var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                pos.z = 0;

                playerShootSingleton.ValueRW.Points[(int)ShootDirectionType.Aim] = pos;
                playerShootSingleton.ValueRW.Points[(int)ShootDirectionType.AimInverse] = pos*-1;

                var skillEventSingleton = SystemAPI.GetSingletonRW<SkillEventSingleton>();
                skillEventSingleton.ValueRW.Events.Add(new SkillEventData
                {
                    Event = SkillEvent.Shoot,
                    Origin = playerE,
                    Target = Entity.Null,
                    Position = playerPos,
                });
            }

        }

    }
}
