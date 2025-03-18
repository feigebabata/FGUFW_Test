using FGUFW;
using UnityEngine;

public static class FGUsing
{

#region ULog
    [System.Diagnostics.Conditional(ULog.Conditional_Log)]
    public static void log(object obj)
    {
        Debug.Log(obj);
    }

    [System.Diagnostics.Conditional(ULog.Conditional_Log)]
    public static void logWarning(object obj)
    {
        Debug.LogWarning(obj);
    }

    [System.Diagnostics.Conditional(ULog.Conditional_Log)]
    public static void logError(object obj)
    {
        Debug.LogError(obj);
        
    }

    /// <summary>
    /// 需要宏 UNITY_ASSERTIONS 开启
    /// </summary>
    /// <param name="mb"></param>
    /// <param name="b"></param>
    /// <param name="msg"></param>
    [System.Diagnostics.Conditional(ULog.Conditional_Log)]
    public static void assert(this MonoBehaviour mb,bool b,string msg)
    {
        UnityEngine.Assertions.Assert.IsTrue(b,msg);
    }
#endregion

#region RandomExtensions
    public static float range(float min=0,float max=1)
    {
        return RandomExtensions.range(min,max);
    }

    public static Vector2 range2(float min=0,float max=1)
    {
        return RandomExtensions.range2(min,max);
    }

    public static Vector3 range3(float min=0,float max=1)
    {
        return RandomExtensions.range3(min,max);
    }

    public static Color rangec(float min=0,float max=1)
    {
        return RandomExtensions.rangec(min,max);
    }
#endregion

#region AssetHelper

    public static T load<T>(string path)
    {
        return AssetHelper.Load<T>(path);
    }

    public static System.Threading.Tasks.Task<T> loadAsync<T>(string path)
    {
        return AssetHelper.LoadAsync<T>(path);
    }

    public static GameObject copy(string path,Transform parent=default)
    {
        return AssetHelper.Copy(path,parent);
    }

    public static System.Threading.Tasks.Task<GameObject> copyAsync<T>(string path,Transform parent=default)
    {
        return AssetHelper.CopyAsync(path,parent);
    }

    public static UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<UnityEngine.ResourceManagement.ResourceProviders.SceneInstance> loadSceneAsync(string path)
    {
        return AssetHelper.LoadSceneAsync(path);
    }

#endregion

#region LitJson
    public static string toJson(object obj)
    {
        return LitJson.JsonMapper.ToJson(obj);
    }
    
    public static T json2Object<T>(string json)
    {
        return LitJson.JsonMapper.ToObject<T>(json);
    }

#endregion

}