using UnityEngine;
using Unity.Entities;

namespace FGUFW.ECS_SpriteAnimator
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class SpriteAnimatorAuthoring : MonoBehaviour
    {
        
    }

    class SpriteAnimatorBaker : Baker<SpriteAnimatorAuthoring>
    {
        public override void Bake(SpriteAnimatorAuthoring authoring)
        {
            
        }
    }


}