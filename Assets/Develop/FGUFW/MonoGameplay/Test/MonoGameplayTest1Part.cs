using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using FGUFW.MonoGameplay;
using UnityEngine;

namespace MonoGameplayTest
{
    
    public class MonoGameplayTest1Part : Part
    {
        private MonoGameplayTestPlay _play;

        public override UniTask OnCreating(Part parent)
        {
            _play = MonoGameplayTestPlay.I;

            addListener();
            return base.OnCreating(parent);
        }

        public override UniTask OnDestroying(Part parent)
        {

            removeListener();
            return base.OnDestroying(parent);
        }

        private void addListener()
        {

        }

        private void removeListener()
        {
            
        }

    }
}

