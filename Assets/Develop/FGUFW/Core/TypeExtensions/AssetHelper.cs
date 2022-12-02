using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;
using System;
using System.Threading.Tasks;

namespace FGUFW
{
    static public class AssetHelper
    {
        static public async void SetSpritePathAsync(this Image self,string path,Action callback=null)
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

        public static Task<GameObject> CopyAsync<T>(string path,Transform parent)
        {
            return Addressables.InstantiateAsync(path,parent).Task;
        }

        public static GameObject Copy<T>(string path,Transform parent)
        {
            return Addressables.InstantiateAsync(path,parent).WaitForCompletion();
        }

    }


}
