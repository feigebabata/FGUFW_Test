using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGUFW.Net;
using System;
using System.Net.Sockets;

namespace FGUFW.Net.Test
{
    public class UdpReceiveTest : MonoBehaviour
    {
        private int i;

        // Start is called before the first frame update
        void Start()
        {
            UdpUtility.On();
            UdpUtility.OnReceive += onReceive;
        }

        private void onReceive(UdpReceiveResult result)
        {
            int val = BitConverter.ToInt32(result.Buffer,0);
            Debug.Log(val);
        }


        /// <summary>
        /// This function is called when the MonoBehaviour will be destroyed.
        /// </summary>
        void OnDestroy()
        {
            UdpUtility.OnReceive -= onReceive;
            UdpUtility.Off();
        }
    }

}