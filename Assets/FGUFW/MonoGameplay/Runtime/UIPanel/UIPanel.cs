using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FGUFW.MonoGameplay
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(CanvasGroup))]
    public class UIPanel : MonoBehaviour
    {
        public float KeepTime;
        public float Progress;
        public CanvasGroup Canvas;
        public Transform Trans;
        private UIPanelEffect[] _uiPanelEffects;
        private Coroutine _progressUpdate;

        void Awake()
        {
            _uiPanelEffects = GetComponents<UIPanelEffect>();
            Canvas = GetComponent<CanvasGroup>();
        }

        public virtual UniTask Show()
        {
            if(_progressUpdate!=null)
            {
                StopCoroutine(_progressUpdate);
            }
            _progressUpdate = StartCoroutine(progressUpdate());

            foreach (var item in _uiPanelEffects)
            {
                item.Show(this);
            }

            if(KeepTime<=0)
            {
                return default;
            }
            else
            {
                return UniTask.Delay((int)(KeepTime*1000));
            }
        }

        private IEnumerator progressUpdate()
        {
            Canvas.interactable = false;
            Progress = 0;
            float startTime = Time.time;
            while (Time.time<startTime+KeepTime)
            {
                yield return null;
                Progress = (Time.time-startTime)/KeepTime;
            }
            Progress = 1;
            _progressUpdate = null;

            Canvas.interactable = true;
        }

        public virtual UniTask Hide()
        {
            if(_progressUpdate!=null)
            {
                StopCoroutine(_progressUpdate);
            }
            _progressUpdate = StartCoroutine(progressUpdate());

            foreach (var item in _uiPanelEffects)
            {
                item.Hide(this);
            }

            if(KeepTime<=0)
            {
                return default;
            }
            else
            {
                return UniTask.Delay((int)(KeepTime*1000));
            }
        }

    }

    public abstract class UIPanelEffect : MonoBehaviour
    {
        public abstract void Show(UIPanel uIPanel);
        
        public abstract void Hide(UIPanel uIPanel);
    }

    
    // public static class PartExtensions
    // {
    //     public static async UniTask<UIPanel> LoadUIPanel( this Part self)
    //     {

    //     }
    // }

}
