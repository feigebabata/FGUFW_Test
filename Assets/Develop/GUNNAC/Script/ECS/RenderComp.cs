
using FGUFW.ECS;
using Unity.Mathematics;
using Unity.Collections;
using UnityEngine;

namespace GUNNAC
{
    
    public struct RenderComp : IComponent
    {
        #region 不可修改
        public int CompType => 11;
        public int EntityUId { get; set; }

        public bool IsCreated { get; private set; }
        #endregion
        //code
        public GameObject GObj; 
        public GameObjectType GObjType;
        // public Vector3 SmoothVelocity;

        public RenderComp(int entityUId=0)
        {
            #region 不可修改
            EntityUId = entityUId;
            
            IsCreated = true;
            #endregion
            //code
            GObj = null;
            GObjType = 0;
            // SmoothVelocity = Vector3.zero;
        }

        public void Dispose()
        {
            #region 不可修改
            IsCreated = false;
            EntityUId = 0;
            
            #endregion
            //code
            GObj = null;
        }
    }
}
