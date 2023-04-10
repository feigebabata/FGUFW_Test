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
    [BurstCompile]
    partial struct ViewFollowSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            EntityQuery eq = new EntityQueryBuilder(Allocator.Temp).WithAll<Player>().Build(ref state);
            state.RequireForUpdate(eq);
        }

        public void OnUpdate(ref SystemState state)
        {
            var entity = SystemAPI.GetSingletonEntity<Player>();
            var localTransform = SystemAPI.GetComponent<LocalTransform>(entity);
            
            foreach (var item in HybridUnityObject.I.ViewFollowItems)
            {
                var pos = localTransform.Position;
                pos.z = item.position.z;
                item.position = pos;
            }
        }


    }
}
//1098 1952 548