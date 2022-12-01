using System.Collections;
using System.Collections.Generic;
using System.Net;
using FGUFW.Net;
using UnityEngine;

public class KcpTest : MonoBehaviour
{
    KcpClient _client;
    
    // Start is called before the first frame update
    void Start()
    {
        IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse("192.168.1.24"),31415);
        _client = new KcpClient(31415,endPoint);
        _client.Send(new byte[]{1,2,3});
    }

    /// <summary>
    /// This function is called when the MonoBehaviour will be destroyed.
    /// </summary>
    void OnDestroy()
    {
        _client.Dispose();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
