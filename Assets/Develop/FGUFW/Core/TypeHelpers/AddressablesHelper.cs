using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;
using System;
using System.Threading.Tasks;

namespace FGUFW
{
    static public class AddressablesHelper
    {
        static public async void SetSpritePathAsync(this Image self,string path,Action callback=null)
        {
            self.sprite = await Addressables.LoadAssetAsync<Sprite>(path).Task;
            callback?.Invoke();
        }
    }


}
