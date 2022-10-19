
using FGUFW.ECS;
using Unity.Mathematics;
using Unity.Collections;

namespace GUNNAC
{
    
    public struct PlayerShootMsgComp : IComponent
    {
        #region 不可修改
        public int CompType => 18;
        public int EntityUId { get; set; }
        public int Dirty { get; set; }

        public bool IsCreated { get; private set; }
        #endregion
        //code
        public int BulletType;


        public PlayerShootMsgComp(int entityUId=0)
        {
            #region 不可修改
            EntityUId = entityUId;
            Dirty = 0;
            IsCreated = true;
            #endregion
            //code
            BulletType = 0;
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
