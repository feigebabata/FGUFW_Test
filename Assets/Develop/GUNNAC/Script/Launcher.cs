using System;
using FGUFW;
using FGUFW.ECS;
using Unity.Mathematics;
using UnityEngine;

namespace GUNNAC
{
    public class Launcher:MonoBehaviour
    {
        public PlayerMoveInputComp PlayerMoveInputComp;
        public PlayerShootInputComp PlayerShootInputComp;
        public GameObject PlayerRender,PlayerCollider,PlayerBullet1;
        public World _world;

        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        void Start()
        {
            _world = new World();
            createPlayer();
            createBattleViewRect();
            initGObjPool();
        }

        /// <summary>
        /// This function is called when the MonoBehaviour will be destroyed.
        /// </summary>
        void OnDestroy()
        {
            _world.Dispose();
        }

        private void createPlayer()
        {
            int playerEUId = _world.CreateEntity(
            (
                ref RenderComp renderComp,
                ref ColliderComp colliderComp,
                ref PositionComp positionComp,
                ref DirectionComp directionComp,
                ref PlayerSizeComp playerSizeComp,
                ref PlayerRenderComp playerRenderComp
            )=>
            {
                var Pos = Vector3.zero;

                renderComp.GObj = PlayerRender.Copy(null);
                renderComp.GObj.transform.position = Pos;

                colliderComp.GObj = PlayerCollider.Copy(null);
                colliderComp.GObj.transform.position = Pos;

                positionComp.Pos.xyz = Pos;
                
                float4 rect = float4.zero;
                var boxCollider = colliderComp.GObj.GetComponent<BoxCollider>();
                rect.x = boxCollider.size.x/2 + boxCollider.center.x; 
                rect.z = boxCollider.size.x/2 - boxCollider.center.x; 
                rect.y = boxCollider.size.z/2 + boxCollider.center.z; 
                rect.w = boxCollider.size.z/2 - boxCollider.center.z;
                playerSizeComp.Rect = rect; 

                playerRenderComp.TailFlameMat = renderComp.GObj.transform.Find("EngineTrails/EngineTrails").GetComponent<MeshRenderer>().material;
                playerRenderComp.PropertyID = Shader.PropertyToID("_TintColor");
            });
            this.PlayerMoveInputComp.PlayerEUId = playerEUId;
            this.PlayerShootInputComp.PlayerEUId = playerEUId;
        }

        private void createBattleViewRect()
        {
            float y = Camera.main.orthographicSize*2;
            float x = y*4/3;
            BattleViewRectComp battleViewRectComp = new BattleViewRectComp();
            battleViewRectComp.Rect = new float4(x/2,y/2,-x/2,-y/2);
            _world.AddOrSetComponent(World.ENTITY_SINGLE,battleViewRectComp);
        }

        private void initGObjPool()
        {
            GameObjectPool.InitPool((int)GameObjectType.PlayerBullet_1,PlayerBullet1,20);
        }

    }
}