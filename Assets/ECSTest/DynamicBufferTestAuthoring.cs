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

    public struct DynamicBufferTest:IComponentData,IEnableableComponent
    {
        public int Value;
    }

    public struct TestBuffer : IBufferElementData
    {
        public int Value;
    }




    public struct Base_A:IComponentData
    {
        public int ID;
    }

    public interface IBase_B
    {
        int ID{get;set;}
    }

    //与Base_A组合使用
    public struct SKill_A:IComponentData
    {

    }


    public struct SKill_B : IComponentData, IBase_B
    {
        public int ID{get;set;}
    }

}
