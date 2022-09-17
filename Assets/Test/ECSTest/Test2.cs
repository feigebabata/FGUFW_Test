
using FGUFW.ECS;
using Unity.Mathematics;
using Unity.Collections;

namespace ECSTest
{
    [GenerateAuthoringComponent]
    public struct Test2 : IComponent
    {
        #region 不可修改
        public int CompType => 2;
        public int EntityUId { get; set; }
        public int Dirty { get; set; }
        public bool IsCreated { get; private set; }
        #endregion
        //code
        public NativeArray<int> Pos;
        public float DeltaTime;
        public int Index;

        public Test2(int entityUId=0)
        {
            #region 不可修改
            EntityUId = entityUId;
            Dirty = 0;
            IsCreated = true;
            #endregion
            //code
            Pos = default(NativeArray<int>);
            DeltaTime = 0;
            Index = 0;
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
