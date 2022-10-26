
using FGUFW.ECS;
using Unity.Mathematics;
using Unity.Collections;

namespace GUNNAC
{
    
    public struct CollisionComp : IComponent
    {
        //不可修改
        public int CompType => 30;
        public int EntityUId { get; set; }
        public int Dirty { get; set; }
        public bool IsCreated { get; private set; }
        //code


        public CollisionComp(int entityUId=0)
        {
            //不可修改
            EntityUId = entityUId;
            IsCreated = true;
            Dirty = 0;
            //code
            
        }

        public void Dispose()
        {
            //region 不可修改
            IsCreated = false;
            EntityUId = 0;
            //code
            
        }
    }
}
