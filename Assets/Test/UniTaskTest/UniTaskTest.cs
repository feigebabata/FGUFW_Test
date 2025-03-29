using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class UniTaskTest : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    async void Start()
    {
        while (true)
        {
            await UniTask.Delay(500);
            Debug.Log(1);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time>2)
        {
            Debug.Log(transform.parent.name);
        }
    }
}
