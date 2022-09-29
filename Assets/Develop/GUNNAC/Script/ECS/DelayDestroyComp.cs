
using FGUFW.ECS;
using Unity.Mathematics;
using Unity.Collections;

namespace GUNNAC
{
    [GenerateAuthoringComponent]
    public struct DelayDestroyComp : IComponent
    {
        #region 不可修改
        public int CompType => 24;
        public int EntityUId { get; set; }
        public bool IsCreated { get; private set; }
        #endregion
        //code
        public int Delay;


        public DelayDestroyComp(int entityUId=0)
        {
            #region 不可修改
            EntityUId = entityUId;
            IsCreated = true;
            #endregion
            //code
            Delay = 0;
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
