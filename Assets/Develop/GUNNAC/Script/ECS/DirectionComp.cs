
using FGUFW.ECS;
using Unity.Mathematics;
using Unity.Collections;

namespace GUNNAC
{
    
    public struct DirectionComp : IComponent
    {
        #region 不可修改
        public int CompType => 10;
        public int EntityUId { get; set; }
        public bool IsCreated { get; private set; }
        #endregion
        //code
        public float4 Forward;

        public DirectionComp(int entityUId=0)
        {
            #region 不可修改
            EntityUId = entityUId;
            IsCreated = true;
            #endregion
            //code
            Forward = float4.zero;
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
