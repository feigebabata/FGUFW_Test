using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using FGUFW.MonoGameplay;
using UnityEngine;

namespace MonoGameplayTest
{
    [UIPanelLoader("Assets/Develop/FGUFW/MonoGameplay/Test/MonoGameplayTestUIPanel.prefab")]
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

        public async override UniTask OnPreload()
        {
            await UniTask.Delay(2000);
            await base.OnPreload();
            Debug.Log(_uiPanel.name);
        }

        private void addListener()
        {

        }

        private void removeListener()
        {
            
        }

    }
}

