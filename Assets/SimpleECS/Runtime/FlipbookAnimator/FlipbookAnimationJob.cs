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

namespace FGUFW.SimpleECS
{
    [BurstCompile]
    public struct FlipbookAnimationPlayJob : IJobParallelFor
    {
        public float DeltaTime;

        [NativeDisableParallelForRestriction]
        public NativeList<FlipbookAnimator> Animators;

        [ReadOnly]
        public NativeList<FlipbookAnimation> Animations;

        [NativeDisableParallelForRestriction]
        public NativeList<FlipbookAnimationUpdate> AnimationUpdates;

        public void Execute(int index)
        {
            var animator = Animators[index];
            if(!animator.Enabled)return;

            var animation = Animations[index];
            if(!animation.Enabled)return;

            if(!animator.Loop && animator.FrameIndex>=animation.Length-1)return;
            
            animator.Time = (animator.Time + DeltaTime*animator.Speed)%animation.Time;
            int newFrameIndex = (int)math.clamp(animator.Time/animation.Time*animation.Length,0,animation.Length-1);

            if(newFrameIndex!=animator.FrameIndex)
            {
                AnimationUpdates[index] = new FlipbookAnimationUpdate
                {
                    Enabled = true,
                    FlipbookIndex = animation.Start+newFrameIndex,
                };
                animator.FrameIndex = newFrameIndex;
            }
            Animators[index] = animator;
        }
    }

    public struct FlipbookAnimationUpdateExecute
    {
        public int FlipbookIndexNameId;
        public NativeList<FlipbookAnimationUpdate> AnimationUpdates;
        public UnorderedList<Renderer> Renderers;

        public void Run(int length)
        {
            for (int i = 0; i < length; i++)
            {
                Execute(i);
            }
        }

        void Execute(int index)
        {
            var animationUpdate = AnimationUpdates[index];
            if(!animationUpdate.Enabled)return;
            
            var renderer = Renderers[index];
            renderer.SetMaterialProperty(FlipbookIndexNameId,animationUpdate.FlipbookIndex);

            animationUpdate.Enabled = false;
            AnimationUpdates[index] = animationUpdate;
        }
    }

}
