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
        private float _playCreatedTime;

        public override async UniTask OnCreating(Part parent)
        {
            I = (T)this;
            Messenger = new OrderedMessenger<Enum>();
            DontDestroyOnLoad(this.gameObject);

            await base.OnCreating(parent);

            Debug.Log($"{this.GetType().Name} Create End.");

            await OnPreload();
            FGUFW.PlayerLoopHelper.AddToLoop<UnityEngine.PlayerLoop.Update>(OnUpdate,this.GetType());
            _playCreatedTime = Time.time;

            Debug.Log($"{this.GetType().Name} Preload End.");
        }

        public override async UniTask OnDestroying(Part parent)
        {
            FGUFW.PlayerLoopHelper.RemoveToLoop<UnityEngine.PlayerLoop.Update>(OnUpdate);
            await base.OnDestroying(parent);
            
            Messenger = null;
            I = null;
        }


        private void OnUpdate()
        {
            _frameData.Index++;
            _frameData.DeltaTime = Time.deltaTime;
            _frameData.WorldTime = Time.time - _playCreatedTime;
            OnUpdate(in _frameData);
        }


    }

}
