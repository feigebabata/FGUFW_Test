using System.Collections;
using System.Collections.Generic;
using FGUFW.ECS;
using Unity.Mathematics;
using UnityEngine;
using GUNNAC;
using System;
using FGUFW;

namespace ECSTest
{
    public class ECSTestComp : MonoBehaviour
    {
        public Transform Player,Target;
        private int _playerEUId,_targetEUId;
        private World _world;
        bool _isRun = false;

        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        IEnumerator Start()
        {
            yield return new WaitForSeconds(1);
            _isRun = true;
            // Application.targetFrameRate = 30;
            _world = new World();

            _targetEUId = _world.CreateEntity((ref PositionComp positionComp)=>
            {
                positionComp.Pos.xyz = Target.position;
            });

            _playerEUId = _world.CreateEntity((ref PositionComp positionComp,ref TargetMoveComp targetMoveComp,ref DirectionComp directionComp)=>
            {
                positionComp.Pos = new float4(Player.position,1);
                targetMoveComp.TargetEUId = _targetEUId;
                targetMoveComp.MoveVelocity = 20;
                targetMoveComp.RotateVelocity = 360*3.5f;
                directionComp.Forward.xyz = Player.forward;
            });
        }

        /// <summary>
        /// This function is called when the MonoBehaviour will be destroyed.
        /// </summary>
        void OnDestroy()
        {
            _world.Dispose();
        }

        /// <summary>
        /// Update is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        void Update()
        {
            if(!_isRun)return;

            PositionComp playerPosComp;
            if(_world.GetComponent(_playerEUId,out playerPosComp))
            {
                // if(playerPosComp.Dirty>0)
                {
                    // Debug.Log(_world.FrameIndex);
                    Player.position = Vector3.Lerp(Player.position,playerPosComp.Pos.xyz,0.5f);
                }
            }
            DirectionComp directionComp;
            if(_world.GetComponent(_playerEUId,out directionComp))
            {
                // if(playerPosComp.Dirty>0)
                {
                    // Debug.Log(_world.FrameIndex);
                    Player.forward = directionComp.Forward.xyz;
                }
            }

            createUpdate();

        }

        private void createUpdate()
        {
            if(Input.GetMouseButtonDown(0))
            {
                Debug.Log(Input.mousePosition);
            }
        }
    }

}
