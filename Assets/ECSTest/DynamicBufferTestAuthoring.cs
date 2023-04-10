using Unity.Entities;
using UnityEngine;
using Unity.Mathematics;

namespace NAME_SAPCE
{
    public class DynamicBufferTestAuthoring:MonoBehaviour
    {
    }

    class DynamicBufferTestBaker : Baker<DynamicBufferTestAuthoring>
    {
        public override void Bake(DynamicBufferTestAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity,new DynamicBufferTest
            {
            });
            AddBuffer<TestBuffer>(entity);
        }
    }

    public struct DynamicBufferTest:IComponentData
    {
        public int Value;
    }

    public struct TestBuffer : IBufferElementData
    {
        public int Value;
    }
}
