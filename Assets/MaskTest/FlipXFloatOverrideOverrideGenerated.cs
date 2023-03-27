using Unity.Entities;
using Unity.Mathematics;

namespace Unity.Rendering
{
    [MaterialProperty("_FlipX")]
    struct FlipXFloatOverride : IComponentData
    {
        public float Value;
    }
}
