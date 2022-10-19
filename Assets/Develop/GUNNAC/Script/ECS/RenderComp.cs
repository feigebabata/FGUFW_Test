
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
        public int Dirty { get; set; }

        public bool IsCreated { get; private set; }
        #endregion
        //code
        public GameObject GObject; 
        

        public RenderComp(int entityUId=0)
        {
            #region 不可修改
            EntityUId = entityUId;
            Dirty = 0;
            IsCreated = true;
            #endregion
            //code
            GObject = null;
        }

        public void Dispose()
        {
            #region 不可修改
            IsCreated = false;
            EntityUId = 0;
            
            #endregion
            //code
            GObject = null;
        }
    }
}
