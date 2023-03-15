using UnityEngine;
using Unity.Entities;
using Unity.Collections;
using Unity.Mathematics;

namespace FGUFW.ECS_SpriteAnimator
{
    [UpdateAfter(typeof(SpriteAnimaionPlaySystem))]
    [UpdateInGroup(typeof(SpriteAnimatorSystemGroup))]
    partial struct SpriteAnimaionEventCreateSystem:ISystem
    {
        
    }
}