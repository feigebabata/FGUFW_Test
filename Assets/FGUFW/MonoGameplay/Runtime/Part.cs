using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using FGUFW;

namespace FGUFW.MonoGameplay
{
    public abstract class Part : MonoBehaviour,IPartUpdate,IPartPreload
    {
        public List<Part> SubParts = new List<Part>();

        protected UIPanel _uiPanel;

        public virtual async UniTask OnCreating(Part parent)
        {
            foreach (var subPart in SubParts)
            {
                await subPart.OnCreating(this);
            }
        }

        public virtual async UniTask OnDestroying(Part parent)
        {
            foreach (var subPart in SubParts)
            {
                await subPart.OnDestroying(this);
            }
            if(_uiPanel)
            {
                Destroy(_uiPanel.gameObject);
            }
            SubParts.Clear();
            Destroy(gameObject);
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

        public T AddPart<T>() where T : Part
        {
            var part = Create<T>(this);
            SubParts.Add(part);
            return part;
        }

        public virtual void OnUpdate(in PlayFrameData playFrameData)
        {
            foreach (var item in SubParts)
            {
                item.OnUpdate(in playFrameData);
            }
        }

        public async virtual UniTask OnPreload()
        {
            _uiPanel = await loadUIPanel();

            foreach (var subPart in SubParts)
            {
                await subPart.OnPreload();
            }
        }

        private async UniTask<UIPanel> loadUIPanel()
        {
            var uiPanelLoader = this.GetAttribute<UIPanelLoaderAttribute>();
            if (uiPanelLoader != null)
            {
                var path = uiPanelLoader.PrefabPath;
                var go = await AssetHelper.CopyAsync(path,null);
                DontDestroyOnLoad(go);
                return go.GetComponent<UIPanel>();
            }
            return default;
        }

        public static T Create<T>(Part parent) where T : Part
        {
            Transform tp = parent==default?null:parent.transform;
            var part = new GameObject(typeof(T).Name).AddComponent<T>();
            DontDestroyOnLoad(part.gameObject);
            part.transform.parent = tp;
            part.transform.localPosition = Vector3.zero;
            return part;
        }
        

    }

}
