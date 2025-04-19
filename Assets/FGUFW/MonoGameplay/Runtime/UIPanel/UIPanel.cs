using System;
using System.Collections;
using System.Collections.Generic;
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
        public CanvasGroup Group;
        public Canvas Canvas;

        /// <summary>
        /// canvas下层 缩放用
        /// </summary>
        public Transform Trans;
        private UIPanelEffect[] _uiPanelEffects;
        private Coroutine _progressUpdate;

        void Awake()
        {
            _uiPanelEffects = GetComponents<UIPanelEffect>();
            Group = GetComponent<CanvasGroup>();
            Canvas = GetComponent<Canvas>();
        }

        public virtual IEnumerator Show(MonoBehaviour mb)
        {
            this.Canvas.enabled = true;
            
            if(_progressUpdate!=null)
            {
                mb.StopCoroutine(_progressUpdate);
            }
            _progressUpdate = mb.StartCoroutine(progressUpdate());

            foreach (var item in _uiPanelEffects)
            {
                item.Show(this);
            }

            yield return _progressUpdate;
        }

        private IEnumerator progressUpdate()
        {
            Group.interactable = false;
            Progress = 0;
            float startTime = Time.time;
            while (Time.time<startTime+KeepTime)
            {
                yield return null;
                Progress = (Time.time-startTime)/KeepTime;
            }
            Progress = 1;
            _progressUpdate = null;

            Group.interactable = true;
        }

        public IEnumerator Hide(MonoBehaviour mb)
        {
            if(_progressUpdate!=null)
            {
                mb.StopCoroutine(_progressUpdate);
            }
            _progressUpdate = mb.StartCoroutine(progressUpdate());
            yield return _progressUpdate;

            foreach (var item in _uiPanelEffects)
            {
                item.Hide(this);
            }

        }

    }

    public abstract class UIPanelEffect : MonoBehaviour
    {
        public abstract void Show(UIPanel uIPanel);
        
        public abstract void Hide(UIPanel uIPanel);
    }


}
