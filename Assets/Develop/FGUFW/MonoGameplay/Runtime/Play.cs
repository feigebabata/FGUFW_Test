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

        public override UniTask OnCreating(Part parent)
        {
            I = (T)this;
            Messenger = new OrderedMessenger<Enum>();
            FGUFW.PlayerLoopHelper.AddToLoop<UnityEngine.PlayerLoop.Update>(OnUpdate,this.GetType());
            _playCreatedTime = Time.time;
            DontDestroyOnLoad(this.gameObject);
            return default;
        }

        public override UniTask OnDestroying(Part parent)
        {
            FGUFW.PlayerLoopHelper.RemoveToLoop<UnityEngine.PlayerLoop.Update>(OnUpdate);
            Messenger = null;
            I = null;
            return default;
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
