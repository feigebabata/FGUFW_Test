
using Unity.Mathematics;
using Unity.Collections;
using UnityEngine;
using FGUFW.ECS;

namespace GUNNAC
{
    [DisallowMultipleComponent]
    public class BattleConfigCompAuthoring:MonoBehaviour, IConvertToComponent
    {
        public UnityEngine.GameObject[] BattleItems;
        public System.Int32[] BattleItemIndex;
        public System.Single[] BattleItemScorllVelocity;

        public void Convert(World world, int entityUId)
        {
            var comp = new BattleConfigComp(entityUId);

            comp.BattleItems = this.BattleItems;
            comp.BattleItemIndex = this.BattleItemIndex;
            comp.BattleItemScorllVelocity = this.BattleItemScorllVelocity;

            world.AddOrSetComponent(entityUId , comp);
        }
    }
}
