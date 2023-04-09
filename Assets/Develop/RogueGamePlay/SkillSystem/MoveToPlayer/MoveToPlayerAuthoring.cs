using Unity.Entities;
using UnityEngine;
using Unity.Mathematics;

namespace RogueGamePlay
{
    public class MoveToPlayerAuthoring:MonoBehaviour
    {
    }

    class MoveToPlayerBaker : Baker<MoveToPlayerAuthoring>
    {
        public override void Bake(MoveToPlayerAuthoring authoring)
        {
            AddComponent(GetEntity(TransformUsageFlags.Dynamic),new MoveToPlayer
            {
            });
        }
    }

    public struct MoveToPlayer:IComponentData
    {
    }
}
