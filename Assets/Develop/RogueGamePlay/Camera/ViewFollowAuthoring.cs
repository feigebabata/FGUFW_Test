using Unity.Entities;
using UnityEngine;
using Unity.Mathematics;

namespace RogueGamePlay
{
    public class ViewFollowAuthoring:MonoBehaviour
    {
        public float3 Offset;
    }

    class ViewFollowBaker : Baker<ViewFollowAuthoring>
    {
        public override void Bake(ViewFollowAuthoring authoring)
        {
            AddComponent(GetEntity(TransformUsageFlags.Dynamic),new ViewFollow
            {
                Offset = authoring.Offset,
            });
        }
    }

    public struct ViewFollow:IComponentData
    {
        public float3 Offset;
    }
}
