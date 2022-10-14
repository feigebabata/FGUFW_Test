using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace FGUFW.Net
{
    public static class UdpUtility
    {
        public static Action<UdpReceiveResult> OnReceive;
        private static UdpClient sendClient,receiveClient;
        private static IPEndPoint broadcastIEP;
        private static bool isOn = false;

        public static void On()
        {
            if(isOn)return;
            AndroidWifi.I.LockAcquire();
            broadcastIEP = new IPEndPoint(IPAddress.Broadcast,PortConfig.BROADCAST_RECEIVE);
            sendClient = new UdpClient(PortConfig.BROADCAST_SEND);
            receiveClient = new UdpClient(PortConfig.BROADCAST_RECEIVE);
            isOn=true;
            Task.Run(loopReceive);
        }

        public static void Off()
        {
            if(!isOn)return;
            AndroidWifi.I.LockRelease();
            isOn = false;
            sendClient.Close();
            receiveClient.Close();
        }

        public async static void Send(byte[] buffer)
        {
            try
            {
                await sendClient.SendAsync(buffer,buffer.Length,broadcastIEP);
            }
            catch (System.Exception){}
        }

        private static void loopReceive()
        {
            while (isOn)
            {
                IPEndPoint remoteEP = new IPEndPoint(0,0);
                byte[] buffer = null;
                try
                {
                    buffer = receiveClient.Receive(ref remoteEP);
                }
                catch (System.Exception ex)
                {
                    if(isOn)
                    {
                        UnityEngine.Debug.LogError(ex.Message);
                    }
                    continue;
                }
                UdpReceiveResult result = new UdpReceiveResult(buffer,remoteEP);
                if(OnReceive!=null)OnReceive(result);
            }
        }

    }
}