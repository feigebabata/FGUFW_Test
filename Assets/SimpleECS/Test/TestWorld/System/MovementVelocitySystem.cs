using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Jobs;
using UnityEngine.Jobs;
using Unity.Burst;
using Unity.Mathematics;
using Unity.Collections;
using System;
using FGUFW;
using FGUFW.SimpleECS;

namespace FGUFW.SimpleECS.Test
{

    public struct MovementVelocitySystem : ISystem<TestWorld>
    {
        public void OnUpdate(TestWorld world, int systemIndex)
        {
            world.Dependency = new Job
            {
                DeltaTime = world.DeltaTime,
                MovementVelocitys = world.MonsterGroup.MovementVelocitys,
            }
            .Schedule(world.MonsterGroup.Transforms,world.Dependency);
        }

        private struct Job : IJobParallelForTransform
        {
            [ReadOnly]
            public NativeList<MovementVelocity> MovementVelocitys;

            public float DeltaTime;

            public void Execute(int index, TransformAccess transform)
            {
                var movementVelocity = MovementVelocitys[index];
                if(movementVelocity.Enabled)
                {
                    transform.position += DeltaTime*movementVelocity.Value;
                }

            }
        }

    }
}