using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Collections;
using Unity.Burst;
using Unity.Transforms;
using FGUFW.Entities;
using FGUFW;

namespace RogueGamePlay
{
    [BurstCompile]
    partial struct MonsterBuilderTestSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            EntityQuery eq = new EntityQueryBuilder(Allocator.Temp).WithAll<Player>().Build(ref state);
            state.RequireForUpdate(eq);
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var playerE = SystemAPI.GetSingletonEntity<Player>();
            var playerLT = SystemAPI.GetComponent<LocalTransform>(playerE);
            var ecb = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);
            foreach (var mbRW in SystemAPI.Query<RefRW<MonsterBuilder>>())
            {
                var mb = mbRW.ValueRO;
                if(mb.LastBuilderTime+mb.IntervalTime>SystemAPI.Time.ElapsedTime)continue;

                
                var random = new Unity.Mathematics.Random();
                random.InitState((uint)SystemAPI.Time.ElapsedTime);

                for (int i = 0; i < mb.Count; i++)
                {
                    var monsterE = ecb.Instantiate(mb.MonsterPrefab);
                    var monsterLT = playerLT;
                    
                    var point = new float2(random.NextFloat(1000,1100),0);
                    var rotate = float4x4.AxisAngle(new float3(0,0,1),random.NextFloat(0,math.PI*2));
                    point = math.mul(new float4(point,0,0),rotate).xy;

                    monsterLT.Position += new float3(point,0);
                    ecb.SetComponent(monsterE,monsterLT);
                }
                mbRW.ValueRW.LastBuilderTime = (float)SystemAPI.Time.ElapsedTime;
            }
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {

        }

    }
}
