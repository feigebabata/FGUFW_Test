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
        public GameObject PlayerRender, PlayerCollider, PlayerBullet1,PlayerBulledCollider1;
        public GameObject BattleRoot;

        private World _world;

        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        void Awake()
        {
            Application.targetFrameRate = 30*2;    
        }

        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        void Start()
        {
            _world = new World();
            createPlayer();
            createBattleViewRect();
            createBattle();
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
                playerRenderComp.ShootPoint = new float4(0, 0, 5+1, 0);
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
            GameObjectPool.InitPool((int)GameObjectType.PlayerBulletCollider_1, PlayerBulledCollider1, 20);
        }

        private void createBattle()
        {
            var battleConfig = GetComponent<BattleConfigCompAuthoring>();
            battleConfig.Convert(_world,World.ENTITY_SINGLE);
            GameObject.Destroy(battleConfig);

            int battleEUId = _world.CreateEntity(
            (
                ref PositionComp positionComp,
                ref RenderComp renderComp,
                ref BattleDataComp battleDataComp
            )=>
            {
                renderComp.GObj = BattleRoot;

            });

        }

    }
}