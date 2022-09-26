
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
        public int Dirty { get; set; }
        public bool IsCreated { get; private set; }
        #endregion
        //code
        public GameObject[] BattleItem;
        public int[] BattleItemCount;
        public float[] BattleItemSize;
        public float[] BattleItemScorllVelocity;

        public BattleConfigComp(int entityUId=0)
        {
            #region 不可修改
            EntityUId = entityUId;
            Dirty = 0;
            IsCreated = true;
            #endregion
            //code
            BattleItem = null;
            BattleItemCount = null;
            BattleItemSize = null;
            BattleItemScorllVelocity = null;
        }

        public void Dispose()
        {
            #region 不可修改
            IsCreated = false;
            EntityUId = 0;
            Dirty = 0;
            #endregion
            //code
            
        }
    }
}
