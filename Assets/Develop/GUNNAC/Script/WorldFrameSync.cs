using FGUFW;
using System;
using System.Collections.Generic;
using UnityEngine;
using FGUFW.Net;
using Google.Protobuf;
using System.Net.Sockets;
using FGUFW.ECS;

namespace GUNNAC
{
    public sealed class WorldFrameSync:IWorldUpdateControl
    {
        public ICmd2Comp Cmd2Comp;
        private FixedList<UInts8> _frameOperates;
        private object _frameOperateLock = new object();
        private int _placeCount,_placeIndex;
        private uint[] _selfCmds;
        private int _syncFrameIndex;
        private bool _singleMode = false;

        private WorldFrameSync(){}

        public WorldFrameSync(int placeCount,int placeIndex)
        {
            _placeIndex = placeIndex;
            _placeCount = placeCount;
            _frameOperates = new FixedList<UInts8>();
            _selfCmds = new uint[8];
            _selfCmds[0]=1;
            UdpUtility.OnReceive += onReceiveUdp;
            UdpUtility.On();
        }

        public void Dispose(World world)
        {
            _frameOperates.Clear();
            _frameOperates = null;
            UdpUtility.OnReceive -= onReceiveUdp;
            UdpUtility.Off();
        }


        private bool getCanSyncFrame(World world)
        {
            //是否已同步
            if(_syncFrameIndex != world.FrameIndex)return false;

            float syncTime = Mathf.Clamp(TimeHelper.DeltaTime,0.016f,0.032f);
            //提前syncTime同步操作
            if(TimeHelper.Time+syncTime < world.NextUpdateTime)return false;

            return true;
        }

        private void syncFrameOperate()
        {
            if(_singleMode)
            {
                UInts8 frameOperate = _frameOperates[_syncFrameIndex];
                
                int index = _syncFrameIndex.RoundIndex(_selfCmds.Length);
                frameOperate[_placeIndex] = _selfCmds[index];
                
                _frameOperates[_syncFrameIndex] = frameOperate;
            }
            else
            {
                PB_Frame frame = new PB_Frame();
                frame.Index = _syncFrameIndex;
                frame.PlaceIndex = _placeIndex;
                for (int i = 0,f_idx=_syncFrameIndex; i < 3 && f_idx>=0; i++,f_idx--)
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
            resetNextCmd();
            _syncFrameIndex++;
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
            int index = (_syncFrameIndex+1).RoundIndex(_selfCmds.Length);
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
                // Debug.Log($"玩家:{frame.PlaceIndex} 逻辑帧索引:{frame.Index}");
            }
        }

        private bool getFrameOperateComplete(World world)
        {
            bool complete = false;
            lock (_frameOperateLock)
            {
                complete = _frameOperates[world.FrameIndex].InitialValue(_placeCount);
            }
            return complete;
        }

        // private void waitFrameOperateComplete(World world)
        // {
        //     while (!getFrameOperateComplete(world))
        //     {
                
        //     }
        // }

        public void OnPreUpdate(World world)
        {
            if(!getCanSyncFrame(world))return;
            
            syncFrameOperate();
        }

        public void OnPreWorldUpdate(World world)
        {
            Cmd2Comp?.Convert(world,_frameOperates[world.FrameIndex]);
        }

        public bool CanUpdate(World world)
        {
           return getFrameOperateComplete(world);
        }
    }
}