using UnityEngine;
using FGUFW.ECS;
using Unity.Mathematics;
using Unity.Collections;

namespace GUNNAC
{
    
    public struct PlayerRenderComp : IComponent
    {
        #region 不可修改
        public int CompType => 16;
        public int EntityUId { get; set; }
        public int Dirty { get; set; }
        public bool IsCreated { get; private set; }
        #endregion
        //code
        public Material TailFlameMat;
        public int PropertyID;
        public float4 ShootPoint;

        public PlayerRenderComp(int entityUId=0)
        {
            #region 不可修改
            EntityUId = entityUId;
            Dirty = 0;
            IsCreated = true;
            #endregion
            //code
            TailFlameMat = null;
            PropertyID = 0;
            ShootPoint = float4.zero;
        }

        public void Dispose()
        {
            #region 不可修改
            IsCreated = false;
            EntityUId = 0;
            Dirty = 0;
            #endregion
            //code
            TailFlameMat = null;

        }
    }
}
