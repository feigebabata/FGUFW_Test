
using Unity.Mathematics;
using Unity.Collections;
using UnityEngine;
using FGUFW.ECS;

namespace ECSTest
{
    [DisallowMultipleComponent]
    public class Test6Authoring:MonoBehaviour, IConvertToComponent
    {
        

        public void Convert(World world, int entityUId)
        {
            var comp = new Test6(entityUId);

            

            world.AddOrSetComponent(entityUId , comp);
        }
    }
}
