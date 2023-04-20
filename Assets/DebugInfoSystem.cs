using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Collections;
using Unity.Burst;
using Unity.Transforms;
using FGUFW.Entities;
using FGUFW.Play;

namespace RogueGamePlay
{
    partial struct DebugInfoSystem : ISystem
    {
        int _seconds;
        int _fps;
        private EntityQuery _eqMonster,_eqBullet;

        public void OnCreate(ref SystemState state)
        {
            _eqMonster = new EntityQueryBuilder(Allocator.Temp).WithAll<Monster>().Build(ref state);
            _eqBullet = new EntityQueryBuilder(Allocator.Temp).WithAll<Attacker>().Build(ref state);
        }

        public void OnUpdate(ref SystemState state)
        {
            if((int)Time.unscaledTime > _seconds)
            {
                ShowFPS.I.FPS = _fps;
                ShowFPS.I.MonsterCount = _eqMonster.ToComponentDataArray<Monster>(Allocator.Temp).Length;
                ShowFPS.I.BulletCount = _eqBullet.ToComponentDataArray<Attacker>(Allocator.Temp).Length;

                _seconds = (int)Time.unscaledTime;
                _fps=0;
            }
            _fps++;
        }

    }
}
