using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using FGUFW.MonoGameplay;
using UnityEngine;

namespace MonoGameplayTest
{
    public class MonoGameplayTestPlay : Play<MonoGameplayTestPlay>
    {
        public override async UniTask OnCreating(Part parent)
        {
            AddPart<MonoGameplayTest1Part>();
            AddPart<MonoGameplayTest2Part>();
            
            // await UniTask.Delay(3000);
            await base.OnCreating(parent);
        }

        public override UniTask OnDestroying(Part parent)
        {

            return base.OnDestroying(parent);
        }
    }
}

