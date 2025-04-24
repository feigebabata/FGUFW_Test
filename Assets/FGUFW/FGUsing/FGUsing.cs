using System.Collections;
using FGUFW;
using LitJson;
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
    /// 否定时抛异常  需要宏 UNITY_ASSERTIONS 开启
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
    public static int range(int min=0,int max=1)
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

    public static int rangeIndex(params float[] props)
    {
        return RandomExtensions.rangeIndex(props);
    }
#endregion

#region AssetHelper

    public static T load<T>(string path)
    {
        return AssetHelper.Load<T>(path);
    }

    public static UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<T> loadAsync<T>(string path)
    {
        return AssetHelper.LoadAsync<T>(path);
    }

    public static GameObject copy(string path,Transform parent=default)
    {
        return AssetHelper.Copy(path,parent);
    }

    public static UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<GameObject> copyAsync(string path,Transform parent=default)
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
        if(obj==default)return default;
        return JsonMapper.ToJson(obj);
        // return JsonUtility.ToJson(obj,true);
    }
    
    public static T json2Object<T>(string json)
    {
        // return JsonUtility.FromJson<T>(json);
        return JsonMapper.ToObject<T>(json);
    }

#endregion

#region GameObject
    public static GameObject createGO(string name=default,Transform parent=default)
    {
        var go = new GameObject(name);
        go.transform.SetParent(parent,false);
        return go;
    }

    public static T createGO<T>(string name=default,Transform parent=default) where T:Component
    {
        var go = new GameObject(name);
        go.transform.SetParent(parent,false);
        var a = go.AddComponent<T>();
        return a;
    }

#endregion

#region Coroutine
    public static WaitForSeconds delayY(float time)
    {
       return new WaitForSeconds(time);
    }

#endregion

#region Editor
    public static void pauseEditor()
    {
       #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPaused = true;
       #endif
    }

#endregion


}