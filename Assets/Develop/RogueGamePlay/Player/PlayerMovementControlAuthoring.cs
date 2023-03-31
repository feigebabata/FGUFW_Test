using Unity.Entities;
using UnityEngine;
using Unity.Mathematics;

namespace RogueGamePlay
{
    public class PlayerMovementControlAuthoring:MonoBehaviour
    {
    }

    class PlayerMovementControlBaker : Baker<PlayerMovementControlAuthoring>
    {
        public override void Bake(PlayerMovementControlAuthoring authoring)
        {
            AddComponent(GetEntity(TransformUsageFlags.Dynamic),new PlayerMovementControl
            {
            });
        }
    }

    public struct PlayerMovementControl:IComponentData
    {
        public float3 Orientation;
    }


}