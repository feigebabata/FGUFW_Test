
using FGUFW.ECS;
using Unity.Mathematics;
using Unity.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GUNNAC
{
    
    public struct BattleDataComp : IComponent
    {
        #region 不可修改
        public int CompType => 19;
        public int EntityUId { get; set; }
        public bool IsCreated { get; private set; }
        #endregion
        //code
        public int BattleItemIndex;
        public Dictionary<int,GameObject> Childs;

        public BattleDataComp(int entityUId=0)
        {
            #region 不可修改
            EntityUId = entityUId;
            IsCreated = true;
            #endregion
            //code
            BattleItemIndex = -1;
            Childs = new Dictionary<int, GameObject>();
        }

        public void Dispose()
        {
            #region 不可修改
            IsCreated = false;
            EntityUId = 0;
            #endregion
            //code
            Childs.Clear();
            Childs = null;
        }
    }
}
