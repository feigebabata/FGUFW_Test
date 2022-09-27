
using FGUFW.ECS;
using Unity.Mathematics;
using Unity.Collections;
using UnityEngine;

namespace GUNNAC
{
    [GenerateAuthoringComponent]
    public struct BattleConfigComp : IComponent
    {
        #region 不可修改
        public int CompType => 20;
        public int EntityUId { get; set; }
        public bool IsCreated { get; private set; }
        #endregion
        //code

        /// <summary>
        /// 战场块模板
        /// </summary>
        public GameObject[] BattleItems;

        /// <summary>
        /// 战场块每个节点的资源
        /// </summary>
        public int[] BattleItemIndex;

        /// <summary>
        /// 战场节点的滚动速度
        /// </summary>
        public float[] BattleItemScorllVelocity;

        public BattleConfigComp(int entityUId=0)
        {
            #region 不可修改
            EntityUId = entityUId;
            IsCreated = true;
            #endregion
            //code
            BattleItems = null;
            BattleItemIndex = null;
            BattleItemScorllVelocity = null;
        }

        public void Dispose()
        {
            #region 不可修改
            IsCreated = false;
            EntityUId = 0;
            #endregion
            //code
            BattleItems = null;
            BattleItemIndex = null;
            BattleItemScorllVelocity = null;
            
        }
    }
}
