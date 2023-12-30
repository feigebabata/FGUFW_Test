using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace FGUFW.MonoGameplay
{
    [RequireComponent(typeof(UIPanel))]
    public class UIPanelActive : UIPanelEffect
    {       

        public override async void Hide(UIPanel uIPanel)
        {
            if(uIPanel.KeepTime>0)await UniTask.Delay((int)(uIPanel.KeepTime*1000));
            uIPanel.gameObject.SetActive(false);
        }

        public override void Show(UIPanel uIPanel)
        {
            uIPanel.gameObject.SetActive(true);
        }
    }
}
