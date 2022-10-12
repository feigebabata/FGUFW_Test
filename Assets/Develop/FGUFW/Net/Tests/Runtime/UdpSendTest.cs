using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGUFW.Net;
using System;

namespace FGUFW.Net.Test
{
    public class UdpSendTest : MonoBehaviour
    {
        private int i;

        // Start is called before the first frame update
        void Start()
        {
            Application.targetFrameRate = 30*2;
            UdpUtility.On();
        }

        // Update is called once per frame
        void Update()
        {
            i++;
            var buffer = BitConverter.GetBytes(i);
            UdpUtility.Send(buffer);
        }

        /// <summary>
        /// This function is called when the MonoBehaviour will be destroyed.
        /// </summary>
        void OnDestroy()
        {
            UdpUtility.Off();
        }
    }

}