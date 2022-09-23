
using FGUFW.ECS;
using Unity.Mathematics;
using Unity.Collections;
using UnityEngine;

namespace GUNNAC
{
    
    public struct ColliderComp : IComponent
    {
        #region 不可修改
        public int CompType => 12;
        public int EntityUId { get; set; }
        public int Dirty { get; set; }
        public bool IsCreated { get; private set; }
        #endregion
        //code
        public GameObject GObj; 
        public GameObjectType GObjType;

        public ColliderComp(int entityUId=0)
        {
            #region 不可修改
            EntityUId = entityUId;
            Dirty = 0;
            IsCreated = true;
            #endregion
            //code
            GObj = null;
            GObjType = 0;
        }

        public void Dispose()
        {
            #region 不可修改
            IsCreated = false;
            EntityUId = 0;
            Dirty = 0;
            #endregion
            //code
            GObj = null;
            
        }
    }
}
