using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace FGUFW.MonoGameplay
{
    public abstract class Part : MonoBehaviour,IPartUpdate
    {
        public List<Part> SubParts = new List<Part>();

        public virtual UniTask OnCreating(Part parent)
        {
            return default;
        }

        public virtual UniTask OnDestroying(Part parent)
        {
            return default;
        }

        public T GetPart<T>() where T : Part
        {
            foreach (var subPart in SubParts)
            {
                if(subPart is T)
                {
                    return (T)subPart;
                }
            }
            return default;
        }

        public void AddPart<T>() where T : Part
        {
            SubParts.Add(Create<T>(this));
        }

        public async UniTask Create(Part parent)
        {
            await OnCreating(parent);

            foreach (var subPart in SubParts)
            {
                await subPart.Create(this);
            }

        }

        public async UniTask Destroy(Part parent)
        {
            foreach (var subPart in SubParts)
            {
                await subPart.Destroy(this);
            }

            await OnDestroying(parent);

            SubParts.Clear();
        }

        public virtual void OnUpdate(in PlayFrameData playFrameData)
        {
            foreach (var item in SubParts)
            {
                item.OnUpdate(in playFrameData);
            }
        }

        public static Part Create<T>(Part parent) where T : Part
        {
            Transform tp = parent==default?null:parent.transform;
            var part = new GameObject(typeof(T).Name).AddComponent<T>();
            part.transform.parent = tp;
            part.transform.localPosition = Vector3.zero;
            return part;
        }

    }

}
