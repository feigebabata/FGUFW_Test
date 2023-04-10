using Unity.Entities;
using UnityEngine;
using Unity.Mathematics;

namespace RogueGamePlay
{
    public class ShootSystemAuthoring:MonoBehaviour
    {
    }

    class ShootSystemBaker : Baker<ShootSystemAuthoring>
    {
        public override void Bake(ShootSystemAuthoring authoring)
        {
            AddComponent(GetEntity(TransformUsageFlags.Dynamic),new ShootSystem
            {
            });
        }
    }

    public struct ShootSystem:IComponentData
    {
    }
}
