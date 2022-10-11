using FGUFW;
using System;
using System.Collections.Generic;
using UnityEngine;
using FGUFW.Net;
using Google.Protobuf;
using System.Net.Sockets;

namespace FGUFW.ECS
{
    public partial class World
    {
        public ICmd2Comp Cmd2Comp;
        private FixedList<UInts8> _frameOperates;
        private object _frameOperateLock = new object();
        private int _operatorCount;
        private uint[] _selfCmds;
        private int _syncFrameIndex;
        private bool _singleMode = true;


        private void onCreateSync(int operatorCount)
        {
            _operatorCount = operatorCount;
            _frameOperates = new FixedList<UInts8>();
            _selfCmds = new uint[8];
            _selfCmds[0]=1;
            UdpUtility.OnReceive += onReceiveUdp;
            UdpUtility.On();
        }

        private void onDestroySync()
        {
            _frameOperates.Clear();
            _frameOperates = null;
            UdpUtility.OnReceive -= onReceiveUdp;
            UdpUtility.Off();
        }

        private void frameSyncUpdate()
        {
            if(_syncFrameIndex!=FrameIndex)return;
            // bool delayEnd = getDelayEnd();
            // if(!delayEnd)return;
            if((int)_maxRanderLength-1!=RenderFrameIndex)return;
            syncFrameOperate();
        }

        private void syncFrameOperate()
        {
            if(_singleMode)
            {
                UInts8 frameOperate = _frameOperates[FrameIndex];
                
                int index = _syncFrameIndex.RoundIndex(_selfCmds.Length);
                
                _frameOperates[FrameIndex] = frameOperate;
            }
            else
            {
                PB_Frame frame = new PB_Frame();
                frame.Index = FrameIndex;
                frame.PlaceIndex = 0;
                for (int i = 0,f_idx=FrameIndex; i < 3 && f_idx>=0; i++,f_idx--)
                {
                    int index = f_idx.RoundIndex(_selfCmds.Length);
                    frame.Cmds.Add(_selfCmds[index]);
                }
                var sendBuffer = frame.ToByteArray();
                for (int i = 0; i < 3; i++)
                {
                    UdpUtility.Send(sendBuffer);
                }
            }
            _syncFrameIndex++;
            resetNextCmd();
        }

        public void PutCmd(uint cmd)
        {
            int index = _syncFrameIndex.RoundIndex(_selfCmds.Length);
            if(_selfCmds[index]==1)_selfCmds[index]=0;
            uint val = _selfCmds[index];
            val = val|cmd;
            _selfCmds[index] = val;
        }

        private void resetNextCmd()
        {
            int index = (FrameIndex+1).RoundIndex(_selfCmds.Length);
            _selfCmds[index] = 1;
        }

        private void onReceiveUdp(UdpReceiveResult result)
        {
            PB_Frame frame = PB_Frame.Parser.ParseFrom(result.Buffer);
            lock (_frameOperateLock)
            {
                int frameIndex = frame.Index;
                int cmdLength = frame.Cmds.Count;
                for (int i = 0; i < cmdLength && frameIndex>=0; i++,frameIndex--)
                {
                    uint cmd = frame.Cmds[i];
                    UInts8 frameOperate = _frameOperates[frameIndex];
                    frameOperate[frame.PlaceIndex] = cmd;
                    _frameOperates[frameIndex] = frameOperate;
                }
            }
        }

        private bool getFrameOperateComplete()
        {
            bool complete = false;
            lock (_frameOperateLock)
            {
                complete = _frameOperates[FrameIndex].InitialValue(_operatorCount);
            }
            return complete;
        }

    }
}