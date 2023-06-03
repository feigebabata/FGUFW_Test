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

    public class MonsterComponentGroup : ComponentGroup
    {
        public NativeList<MovementVelocity> MovementVelocitys;
        public NativeList<FlipbookAnimator> FlipbookAnimators;
        public NativeList<FlipbookAnimation> FlipbookAnimations;
        public NativeList<FlipbookAnimationUpdate> FlipbookAnimationUpdates;
        public UnorderedList<Renderer> Renderers;

        public MonsterComponentGroup(GameObject prefab, int capacity = 64) : base(prefab, capacity)
        {
            if(capacity<0)capacity=64;
            MovementVelocitys = new NativeList<MovementVelocity>(capacity,Allocator.Persistent);
            FlipbookAnimators = new NativeList<FlipbookAnimator>(capacity,Allocator.Persistent);
            FlipbookAnimations = new NativeList<FlipbookAnimation>(capacity,Allocator.Persistent);
            FlipbookAnimationUpdates = new NativeList<FlipbookAnimationUpdate>(capacity,Allocator.Persistent);
            Renderers = new UnorderedList<Renderer>(capacity);
        }

        protected override void OnDispose()
        {
            MovementVelocitys.Dispose();
            FlipbookAnimators.Dispose();
            FlipbookAnimations.Dispose();
            FlipbookAnimationUpdates.Dispose();
            Renderers.Dispose();
        }

        protected override void OnDestroyEntity(int entityIndex)
        {
            MovementVelocitys.RemoveAtSwapBack(entityIndex);
            FlipbookAnimators.RemoveAtSwapBack(entityIndex);
            FlipbookAnimations.RemoveAtSwapBack(entityIndex);
            FlipbookAnimationUpdates.RemoveAtSwapBack(entityIndex);
            Renderers.RemoveAt(entityIndex);
        }

        protected override void OnInstantiate(int id,Transform t)
        {
            t.position = Vector3.zero;
            t.name = $"Monster:{id}";

            MovementVelocitys.Add(t.GetComponent<MovementVelocityAuthoring>().Convert());
            FlipbookAnimators.Add(t.GetComponent<FlipbookAnimatorAuthoring>().Convert());
            FlipbookAnimations.Add(t.GetComponent<FlipbookAnimationAuthoring>().Convert());
            FlipbookAnimationUpdates.Add(new FlipbookAnimationUpdate());
            Renderers.Add(t.GetComponent<Renderer>());
        }

    }
}