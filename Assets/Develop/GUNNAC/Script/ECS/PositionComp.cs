
using FGUFW.ECS;
using Unity.Mathematics;
using Unity.Collections;

namespace GUNNAC
{
    
    public struct PositionComp : IComponent
    {
        #region 不可修改
        public int CompType => 2;
        public int EntityUId { get; set; }

        public bool IsCreated { get; private set; }
        #endregion
        //code

        public float4 Pos;
        public float4 PrevPos;

        public PositionComp(int entityUId=0)
        {
            #region 不可修改
            EntityUId = entityUId;
            
            IsCreated = true;
            #endregion
            //code
            Pos = float4.zero;
            PrevPos = float4.zero;
        }

        public void Dispose()
        {
            #region 不可修改
            IsCreated = false;
            EntityUId = 0;
            
            #endregion
            //code
            
        }
    }
}
