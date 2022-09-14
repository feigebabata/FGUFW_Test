
using FGUFW.ECS;
using Unity.Mathematics;
using Unity.Collections;

namespace ECSTest
{
    public struct Test3 : IComponent
    {
        #region 不可修改
        public int CompType { get; set; }
        public int EntityUId { get; set; }
        public int Dirty { get; set; }
        public bool IsCreated { get; private set; }
        #endregion
        //code


        public Test3(int entityUId)
        {
            #region 不可修改
            CompType = 6;
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
