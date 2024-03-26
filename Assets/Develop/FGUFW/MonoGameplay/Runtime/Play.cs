#define FIXED_UPDATE

using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using FGUFW;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace FGUFW.MonoGameplay
{
    public abstract class Play<T>:Part where T:Play<T>
    {
        public static T I;
        public IOrderedMessenger<Enum> Messenger;

        [SerializeField]
        private PlayFrameData _frameData;
        public PlayFrameData FrameData=>_frameData;
        private float _playCreatedTime;

        public override async UniTask OnCreating(Part parent)
        {
            I = (T)this;
            Messenger = new OrderedMessenger<Enum>();

            await base.OnCreating(parent);

            Debug.Log($"{this.GetType().Name} Create End.");

            await OnPreload();
            #if FIXED_UPDATE
                FGUFW.PlayerLoopHelper.AddToLoop<UnityEngine.PlayerLoop.FixedUpdate>(OnUpdate,this.GetType());
                _playCreatedTime = Time.fixedTime;
            #else
                FGUFW.PlayerLoopHelper.AddToLoop<UnityEngine.PlayerLoop.Update>(OnUpdate,this.GetType());
                _playCreatedTime = Time.time;
            #endif

            Debug.Log($"{this.GetType().Name} Preload End.");

            
            UnityEngine.Application.quitting += onAppQuiting;
        }

        public override async UniTask OnDestroying(Part parent)
        {
            #if FIXED_UPDATE
                FGUFW.PlayerLoopHelper.RemoveToLoop<UnityEngine.PlayerLoop.FixedUpdate>(OnUpdate);
            #else
                FGUFW.PlayerLoopHelper.RemoveToLoop<UnityEngine.PlayerLoop.Update>(OnUpdate);
            #endif
            await base.OnDestroying(parent);
            
            UnityEngine.Application.quitting -= onAppQuiting;
            Messenger = null;
            I = null;

            Debug.Log($"{this.GetType().Name} Destroy End.");
        }


        private void OnUpdate()
        {
            _frameData.Index++;
            _frameData.DeltaTime = Time.fixedDeltaTime;
            _frameData.WorldTime = Time.fixedTime - _playCreatedTime;
            OnUpdate(in _frameData);
        }

        private async void onAppQuiting()
        {
            #if UNITY_EDITOR
            await I.OnDestroying(default);
            #endif
        }

    }

}
