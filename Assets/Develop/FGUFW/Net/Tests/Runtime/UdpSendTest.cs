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
            Application.targetFrameRate = 30*1;
            UdpUtility.On();
        }

        // Update is called once per frame
        void Update()
        {
            i++;
            if(i%30!=0)return;
            var buffer = BitConverter.GetBytes(i);
            DateTime.UtcNow.SetRecord();
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