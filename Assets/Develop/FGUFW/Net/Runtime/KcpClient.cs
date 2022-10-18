using System.Buffers;
using System.Net;
using System.Net.Sockets;
using System.Net.Sockets.Kcp;
using Kcp = System.Net.Sockets.Kcp.UnSafeSegManager.Kcp;
using System;
using System.Threading.Tasks;

namespace FGUFW.Net
{
    public class KcpClient : IKcpCallback,IDisposable
    {
        private Kcp _kcp;
        private UdpClient _client;
        private IPEndPoint _endPoint,_remoteEndPoint;
        private bool _runing;

        public void Output(IMemoryOwner<byte> buffer, int avalidLength)
        {
            
        }

        public KcpClient(int port, IPEndPoint endPoint)
        {
            _endPoint = endPoint;
            _remoteEndPoint = new IPEndPoint(IPAddress.Any,0);
            _client = new UdpClient(port);
            _kcp = new Kcp(314,this);
            _runing = true;
            Task.Run(loopReceive);
        }

        public void Dispose()
        {
            _runing = false;
            _client.Dispose();
            _client = null;
            _kcp.Dispose();
            _kcp=null;
        }

        private void loopReceive()
        {
            while (_runing)
            {
                byte[] buffer = null;
                try
                {
                    buffer = _client.Receive(ref _remoteEndPoint);
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
    }
}