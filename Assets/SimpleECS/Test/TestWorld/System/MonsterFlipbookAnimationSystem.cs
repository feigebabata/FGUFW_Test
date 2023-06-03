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

    public struct MonsterFlipbookAnimationSystem : ISystem<TestWorld>,ISystemOnCreate<TestWorld>
    {
        private int _flipbookIndexNameId;

        public void OnCreate(TestWorld world, int systemIndex)
        {
            _flipbookIndexNameId = Shader.PropertyToID("_FlipbookIndex");
        }

        public void OnUpdate(TestWorld world, int systemIndex)
        {
            world.Dependency = new FlipbookAnimationPlayJob
            {
                DeltaTime = world.DeltaTime,
                Animators = world.MonsterGroup.FlipbookAnimators,
                Animations = world.MonsterGroup.FlipbookAnimations,
                AnimationUpdates = world.MonsterGroup.FlipbookAnimationUpdates,
            }
            .Schedule(world.MonsterGroup.Length,64,world.Dependency);
            world.Dependency.Complete();

            new FlipbookAnimationUpdateExecute
            {
                FlipbookIndexNameId = _flipbookIndexNameId,
                AnimationUpdates = world.MonsterGroup.FlipbookAnimationUpdates,
                Renderers = world.MonsterGroup.Renderers,
            }
            .Run(world.MonsterGroup.Length);

        }



    }
}