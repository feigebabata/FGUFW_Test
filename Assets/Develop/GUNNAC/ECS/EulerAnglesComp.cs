
using FGUFW.ECS;
using Unity.Mathematics;
using Unity.Collections;

namespace GUNNAC
{
    
    public struct EulerAnglesComp : IComponent
    {
        #region 不可修改
        public int CompType => 3;
        public int EntityUId { get; set; }
        public int Dirty { get; set; }
        public bool IsCreated { get; private set; }
        #endregion
        //code
        public float4 EulerAngles;

        public EulerAnglesComp(int entityUId=0)
        {
            #region 不可修改
            EntityUId = entityUId;
            Dirty = 0;
            IsCreated = true;
            #endregion
            //code
            EulerAngles = float4.zero;
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
