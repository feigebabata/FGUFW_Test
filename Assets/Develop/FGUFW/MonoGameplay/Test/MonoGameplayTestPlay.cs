using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using FGUFW.MonoGameplay;
using UnityEngine;

namespace MonoGameplayTest
{
    public class MonoGameplayTestPlay : Play<MonoGameplayTestPlay>
    {
        public override UniTask OnCreating(Part parent)
        {
            AddPart<MonoGameplayTest1Part>();
            AddPart<MonoGameplayTest2Part>();
            
            return base.OnCreating(parent);
        }

        public override UniTask OnDestroying(Part parent)
        {

            return base.OnDestroying(parent);
        }
    }
}

