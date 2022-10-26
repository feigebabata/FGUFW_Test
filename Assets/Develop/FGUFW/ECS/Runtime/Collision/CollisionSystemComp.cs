using FGUFW;
using FGUFW.ECS;
using Unity.Mathematics;
using Unity.Collections;

namespace GUNNAC
{
    
    public struct CollisionSystemComp : IComponent
    {
        //不可修改
        public int CompType => 31;
        public int EntityUId { get; set; }
        public int Dirty { get; set; }
        public bool IsCreated { get; private set; }
        //code
        public UnorderedList<CollisionComp> Collisions;

        public CollisionSystemComp(int entityUId=0)
        {
            //不可修改
            EntityUId = entityUId;
            IsCreated = true;
            Dirty = 0;
            //code
            Collisions = new UnorderedList<CollisionComp>();
            
        }

        public void Dispose()
        {
            //region 不可修改
            IsCreated = false;
            EntityUId = 0;
            //code
            Collisions.Clear();
            Collisions=null;
        }
    }
}
