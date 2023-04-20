using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Collections;
using Unity.Burst;
using Unity.Transforms;
using NAME_SAPCE;

namespace NAME_SPACE
{
    [BurstCompile]
    partial struct DynamicBufferTestSystem : ISystem
    {
        private ComponentLookup<DynamicBufferTest> _compLockUp;
        private EntityQuery _eq;
        private Entity _entity;

        public void OnCreate(ref SystemState state)
        {
            _compLockUp = state.GetComponentLookup<DynamicBufferTest>(false);
            _eq = new EntityQueryBuilder(Allocator.Temp).WithAll<DynamicBufferTest>().Build(ref state);
        }

        public void OnUpdate(ref SystemState state)
        {
            // _compLockUp.Update(ref state);

            // var comps = _eq.ToEntityArray(Allocator.Temp);
            // Debug.Log(comps.Length);

            // foreach (var (item,entity) in SystemAPI.Query<DynamicBufferTest>().WithEntityAccess())
            // {
            //     SystemAPI.SetComponentEnabled<DynamicBufferTest>(entity,false);
            //     Debug.Log(item);
            //     _entity = entity;
            // }
            // Debug.Log(_compLockUp[_entity]);

        }
    }
}
