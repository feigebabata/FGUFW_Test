
using FGUFW.ECS;
using Unity.Mathematics;
using Unity.Collections;

namespace GUNNAC
{
    public struct EnemyCreateComp : IComponent
    {
        #region 不可修改
        public int CompType => 25;
        public int EntityUId { get; set; }
        public int Dirty { get; set; }
        public bool IsCreated { get; private set; }
        #endregion
        //code
        public int Delay;
        public int PrevCreateTime;

        public EnemyCreateComp(int entityUId=0)
        {
            #region 不可修改
            EntityUId = entityUId;
            Dirty = 0;
            IsCreated = true;
            #endregion
            //code
            Delay = 0;
            PrevCreateTime = 0;
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
