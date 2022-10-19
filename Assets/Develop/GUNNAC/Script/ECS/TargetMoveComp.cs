
using FGUFW.ECS;
using Unity.Mathematics;
using Unity.Collections;

namespace GUNNAC
{
    
    public struct TargetMoveComp : IComponent
    {
        #region 不可修改
        public int CompType => 7;
        public int EntityUId { get; set; }
        public int Dirty { get; set; }

        public bool IsCreated { get; private set; }
        #endregion
        //code
        public int TargetEUId;
        public float MoveVelocity;
        public float RotateVelocity;

        public TargetMoveComp(int entityUId=0)
        {
            #region 不可修改
            EntityUId = entityUId;
            Dirty = 0;
            IsCreated = true;
            #endregion
            //code
            TargetEUId = 0;
            MoveVelocity = 0;
            RotateVelocity = 0;
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
