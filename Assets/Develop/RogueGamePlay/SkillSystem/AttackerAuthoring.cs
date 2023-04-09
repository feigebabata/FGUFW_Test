using Unity.Entities;
using UnityEngine;
using Unity.Mathematics;

namespace RogueGamePlay
{
    public class AttackerAuthoring:MonoBehaviour
    {
        
    }

    class AttackerBaker : Baker<AttackerAuthoring>
    {
        public override void Bake(AttackerAuthoring authoring)
        {
            AddComponent(GetEntity(TransformUsageFlags.Dynamic),new Attacker
            {
            });
        }
    }

    public struct Attacker:IComponentData
    {
    }
}
