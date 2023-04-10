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

namespace RogueGamePlay
{
    [BurstCompile]
    partial struct AttackComputeSystem : ISystem
    {
        ComponentLookup<Monster> _monsters;
        ComponentLookup<Attacker> _attackers;

        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            _monsters = state.GetComponentLookup<Monster>(false);
            _attackers = state.GetComponentLookup<Attacker>(false);

            EntityQuery eq = new EntityQueryBuilder(Allocator.Temp).WithAll<Player>().Build(ref state);
            state.RequireForUpdate(eq);
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            _monsters.Update(ref state);
            _attackers.Update(ref state);

            state.Dependency = new Job
            {
                Player = SystemAPI.GetSingletonRW<Player>(),
                PlayerEntity = SystemAPI.GetSingletonEntity<Player>(),
                Monsters = _monsters,
                Attackers = _attackers,
            }
            .Schedule(SystemAPI.GetSingleton<SimulationSingleton>(),state.Dependency);
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {

        }

        [BurstCompile]
        partial struct Job : ITriggerEventsJob
        {
            [ReadOnly]
            public ComponentLookup<Monster> Monsters;

            [ReadOnly]
            public ComponentLookup<Attacker> Attackers;

            [ReadOnly]
            public Entity PlayerEntity;

            public RefRW<Player> Player;

            public void Execute(TriggerEvent triggerEvent)
            {
                var e_A = triggerEvent.EntityA;
                var e_B = triggerEvent.EntityB;

                if(e_A==PlayerEntity && Attackers.HasComponent(e_B))
                {
                    attackPlayer(e_B);
                }
                if(e_B==PlayerEntity && Attackers.HasComponent(e_A))
                {
                    attackPlayer(e_A);
                }
                
                if(Attackers.HasComponent(e_A) && Monsters.HasComponent(e_B))
                {
                    attackMonster(e_A,e_B);
                }
                if(Attackers.HasComponent(e_B) && Monsters.HasComponent(e_A))
                {
                    attackMonster(e_B,e_A);
                }

            }

            void attackPlayer(Entity attackerE)
            {
                var attacker = Attackers[attackerE];
                var hp = Player.ValueRW.HP-attacker.Power;
                Player.ValueRW.HP = math.clamp(hp,0,float.MaxValue);
                attacker.AttackCount--;
                Attackers[attackerE] = attacker;
            }

            void attackMonster(Entity attackerE,Entity monsterE)
            {
                var attacker = Attackers[attackerE];
                var monster = Monsters[monsterE];
                var hp = monster.HP-attacker.Power;
                monster.HP = math.clamp(hp,0,float.MaxValue);
                Monsters[monsterE] = monster;
                attacker.AttackCount--;
                Attackers[attackerE] = attacker;
            }

        }
    }
}
