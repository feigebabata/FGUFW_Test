
using FGUFW.ECS;
using Unity.Mathematics;
using Unity.Collections;
using UnityEngine;

namespace GUNNAC
{
    
    public struct PoolRenderComp : IComponent,IGameObjectPoolItem
    {
        #region 不可修改
        public int CompType => 27;
        public int EntityUId { get; set; }
        public bool IsCreated { get; private set; }
        public GameObject GObject { get; private set; }
        public int ItemType { get; private set; }
        #endregion
        //code


        public PoolRenderComp(int entityUId=0,int itemType=0)
        {
            #region 不可修改
            EntityUId = entityUId;
            IsCreated = true;
            #endregion
            //code
            ItemType = itemType;
            GObject = GameObjectPool.Get(ItemType);
            GObject.name = $"{EntityUId}_{ItemType}";
        }

        public void Dispose()
        {
            #region 不可修改
            IsCreated = false;
            EntityUId = 0;
            #endregion
            //code
            GameObjectPool.ReCycle(ItemType,GObject);
            ItemType = 0;
            GObject = null;
        }
    }
}
