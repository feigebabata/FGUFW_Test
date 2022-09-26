
using FGUFW.ECS;
using Unity.Mathematics;
using Unity.Collections;

namespace GUNNAC
{
    
    public struct PlayerSizeComp : IComponent
    {
        #region 不可修改
        public int CompType => 15;
        public int EntityUId { get; set; }

        public bool IsCreated { get; private set; }
        #endregion
        //code
        /// <summary>
        /// x:Right,y:Top,z:Left,w:Bottom
        /// </summary>
        public float4 Rect;


        public PlayerSizeComp(int entityUId=0)
        {
            #region 不可修改
            EntityUId = entityUId;
            
            IsCreated = true;
            #endregion
            //code
            Rect = float4.zero;

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
