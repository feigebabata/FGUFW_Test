
using Unity.Mathematics;
using Unity.Collections;
using UnityEngine;
using FGUFW.ECS;

namespace GUNNAC
{
    [DisallowMultipleComponent]
    public class BattleConfigCompAuthoring:MonoBehaviour, IConvertToComponent
    {
        public UnityEngine.GameObject[] BattleItem;
        public System.Int32[] BattleItemCount;
        public System.Single[] BattleItemSize;
        public System.Single[] BattleItemScorllVelocity;

        public void Convert(World world, int entityUId)
        {
            var comp = new BattleConfigComp(entityUId);

            comp.BattleItem = this.BattleItem;
            comp.BattleItemCount = this.BattleItemCount;
            comp.BattleItemSize = this.BattleItemSize;
            comp.BattleItemScorllVelocity = this.BattleItemScorllVelocity;

            world.AddOrSetComponent(entityUId , comp);
        }
    }
}
