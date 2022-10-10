using FGUFW;
using System;
using System.Collections.Generic;
using UnityEngine;
using FGUFW.Net;

namespace FGUFW.ECS
{
    public partial class World
    {
        private FixedList<FrameOperate> _frameOperates;
        private int _operatorCount;

        private void onCreateSync(int operatorCount)
        {
            _operatorCount = operatorCount;
            _frameOperates = new FixedList<FrameOperate>();
            UdpUtility.On();
        }

        private void onDestroySync()
        {
            _frameOperates.Clear();
            _frameOperates = null;
            UdpUtility.Off();
        }

        private void frameSyncUpdate()
        {
            bool delayEnd = getDelayEnd();
            if(!delayEnd)return;

        }

        private void syncFrameOperate()
        {

        }

        public void PutCmd()
        {
            
        }

    }
}