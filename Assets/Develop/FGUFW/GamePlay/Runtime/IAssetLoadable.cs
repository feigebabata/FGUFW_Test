using System.Collections.Generic;
using System.Threading.Tasks;
using FGUFW;
using UnityEngine;

namespace FGUFW.GamePlay
{
    public interface IAssetLoadable
    {

    }

    public static class IAssetLoadableExtensions
    {
        public static GameObject Instantiate(this IAssetLoadable self,string path,Transform parent)
        {
            return AssetHelper.Copy(path,parent);
        }

        public static Task<GameObject> InstantiateAsync(this IAssetLoadable self,string path,Transform parent)
        {
            return AssetHelper.CopyAsync(path,parent);
        }

        public static T LoadAsset<T>(this IAssetLoadable self,string path)
        {
            return AssetHelper.Load<T>(path);
        }

        public static Task<T> LoadAssetAsync<T>(this IAssetLoadable self,string path)
        {
            return AssetHelper.LoadAsync<T>(path);
        }
    }
}