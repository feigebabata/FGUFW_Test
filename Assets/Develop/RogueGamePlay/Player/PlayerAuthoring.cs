using Unity.Entities;
using UnityEngine;
using Unity.Mathematics;

namespace RogueGamePlay
{
    public class PlayerAuthoring:MonoBehaviour
    {
    }

    class PlayerBaker : Baker<PlayerAuthoring>
    {
        public override void Bake(PlayerAuthoring authoring)
        {
            AddComponent(GetEntity(TransformUsageFlags.Dynamic),new Player
            {
            });
        }
    }

    public struct Player:IComponentData
    {
    }
}
