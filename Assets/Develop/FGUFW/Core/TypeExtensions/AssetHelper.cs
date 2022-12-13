using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;
using System;
using System.Threading.Tasks;

namespace FGUFW
{
    public static class AssetHelper
    {
        public static async void SetSpritePathAsync(this Image self,string path,Action callback=null)
        {
            self.sprite = await Addressables.LoadAssetAsync<Sprite>(path).Task;
            callback?.Invoke();
        }

        public static Task<T> LoadAsync<T>(string path)
        {
            return Addressables.LoadAssetAsync<T>(path).Task;
        }

        public static T Load<T>(string path)
        {
            return Addressables.LoadAssetAsync<T>(path).WaitForCompletion();
        }

        public static Task<GameObject> CopyAsync(string path,Transform parent)
        {
            return Addressables.InstantiateAsync(path,parent).Task;
        }

        public static GameObject Copy(string path,Transform parent)
        {
            return Addressables.InstantiateAsync(path,parent).WaitForCompletion();
        }

    }


}
