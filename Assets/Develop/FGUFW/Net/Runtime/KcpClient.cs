using System.Buffers;
using System.Net;
using System.Net.Sockets;
using System.Net.Sockets.Kcp;
using Kcp = System.Net.Sockets.Kcp.UnSafeSegManager.Kcp;
using System;
using System.Threading.Tasks;
using System.Threading;

namespace FGUFW.Net
{
    public class KcpClient : IKcpCallback,IDisposable
    {
        public Action<byte[]> OnReceive;

        private Kcp _kcp;
        private UdpClient _udpClient;
        private IPEndPoint _endPoint,_remoteEndPoint;
        private bool _runing;

        public void Output(IMemoryOwner<byte> buffer, int avalidLength)
        {
            byte[] dgram = buffer.Memory.Slice(0,avalidLength).ToArray();
            _udpClient.SendAsync(dgram,avalidLength,_endPoint);
        }

        public KcpClient(int port, IPEndPoint endPoint)
        {
            _endPoint = endPoint;
            _remoteEndPoint = new IPEndPoint(IPAddress.Any,0);
            _udpClient = new UdpClient(port);
            _runing = true;
            _kcp = new Kcp(314,this);
            _kcp.NoDelay(1,10,2,1);
            Task.Run(loopKcpUpdate);
            Task.Run(loopKcpInput);
            Task.Run(loopReceive);
        }

        public void Dispose()
        {
            _runing = false;
            _udpClient.Dispose();
            _udpClient = null;
            _kcp.Dispose();
            _kcp=null;
        }

        public void Send(byte[] buffer)
        {
            _kcp.Send(buffer);
        }

        private async void loopKcpUpdate()
        {
            while (_runing)
            {
                _kcp.Update(DateTimeOffset.UtcNow);
                await Task.Delay(4);
            }
        }

        private async void loopKcpInput()
        {
            while (_runing)
            {
                byte[] buffer = null;
                try
                {
                    buffer = (await _udpClient.ReceiveAsync()).Buffer;
                }
                catch (System.Exception ex)
                {
                    if(_runing)
                    {
                        UnityEngine.Debug.LogError(ex.Message);
                    }
                    continue;
                }
                
                if(buffer!=null && buffer.Length>0)
                {
                    _kcp.Input(buffer);
                }
            }
        }

        private void loopReceive()
        {
            while (_runing)
            {
                var (buffer, avalidLength) = _kcp.TryRecv();
                if (buffer != null)
                {
                    var bytes = buffer.Memory.Slice(0, avalidLength).ToArray();
                    if(OnReceive!=null)OnReceive(bytes);
                }
            }
        }

    }
}