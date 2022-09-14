
using FGUFW.ECS;
using Unity.Mathematics;
using Unity.Collections;

namespace ECSTest
{
    public struct Test4 : IComponent
    {
        #region 不可修改
        public int CompType { get; set; }
        public int EntityUId { get; set; }
        public int Dirty { get; set; }
        public bool IsCreated { get; private set; }
        #endregion
        //code


        public Test4(int entityUId)
        {
            #region 不可修改
            CompType = 7;
            EntityUId = entityUId;
            Dirty = 0;
            IsCreated = true;
            #endregion
            //code
            
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