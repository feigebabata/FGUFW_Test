
using Unity.Mathematics;
using Unity.Collections;
using UnityEngine;
using FGUFW.ECS;

namespace ECSTest
{
    [DisallowMultipleComponent]
    public class Test2Authoring:MonoBehaviour, IConvertToComponent
    {
        public System.Single DeltaTime;
        public System.Int32 Index;

        public void Convert(World world, int entityUId)
        {
            var comp = new Test2(entityUId);

            comp.DeltaTime = this.DeltaTime;
            comp.Index = this.Index;

            world.AddOrSetComponent(entityUId , comp);
        }
    }
}
