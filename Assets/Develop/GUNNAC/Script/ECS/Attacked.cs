
using FGUFW.ECS;
using Unity.Mathematics;
using Unity.Collections;

namespace GUNNAC
{
    
    public struct Attacked : IComponent
    {
        #region 不可修改
        public int CompType => 5;
        public int EntityUId { get; set; }
        public bool IsCreated { get; private set; }
        #endregion
        //code
        public int Level;

        public Attacked(int entityUId=0)
        {
            #region 不可修改
            EntityUId = entityUId;
            IsCreated = true;
            #endregion
            //code
            Level = 0;
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
