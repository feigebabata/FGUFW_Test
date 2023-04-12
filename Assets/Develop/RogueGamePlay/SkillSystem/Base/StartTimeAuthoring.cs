using Unity.Entities;
using UnityEngine;
using Unity.Mathematics;

namespace RogueGamePlay
{
    public class StartTimeAuthoring:MonoBehaviour
    {
    }

    class StartTimeBaker : Baker<StartTimeAuthoring>
    {
        public override void Bake(StartTimeAuthoring authoring)
        {
            AddComponent(GetEntity(TransformUsageFlags.Dynamic),new StartTime
            {
            });
        }
    }

    public struct StartTime:IComponentData
    {
        public float Time;
    }
}
