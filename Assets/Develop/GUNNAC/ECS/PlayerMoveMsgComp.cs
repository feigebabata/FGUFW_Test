
using FGUFW.ECS;
using Unity.Mathematics;
using Unity.Collections;

namespace GUNNAC
{
    
    public struct PlayerMoveMsgComp : IComponent
    {
        #region 不可修改
        public int CompType => 8;
        public int EntityUId { get; set; }
        public int Dirty { get; set; }
        public bool IsCreated { get; private set; }
        #endregion
        //code
        public float4 MoveDelta;

        public PlayerMoveMsgComp(int entityUId=0)
        {
            #region 不可修改
            EntityUId = entityUId;
            Dirty = 0;
            IsCreated = true;
            #endregion
            //code
            MoveDelta = float4.zero;
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
