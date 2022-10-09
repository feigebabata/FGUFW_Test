
using Unity.Mathematics;
using Unity.Collections;
using UnityEngine;
using FGUFW.ECS;

namespace GUNNAC
{
    [DisallowMultipleComponent]
    public class DelayDestroyCompAuthoring:MonoBehaviour, IConvertToComponent
    {
        public System.Int32 Delay;

        public void Convert(World world, int entityUId)
        {
            var comp = new DelayDestroyComp(entityUId);

            comp.Delay = this.Delay;

            world.AddOrSetComponent(entityUId , comp);
        }
    }
}
