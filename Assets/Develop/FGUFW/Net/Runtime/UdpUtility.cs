using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace FGUFW.Net
{
    public static class UdpUtility
    {
        public static Action<UdpReceiveResult> OnReceive;
        private static UdpClient client;
        private static IPEndPoint broadcastIEP;
        private static bool isOn = false;

        public static void On(IPAddress targetIP,int targetPort=PortConfig.BROADCAST_RECEIVE)
        {
            if(isOn)return;
            AndroidWifi.I.LockAcquire();
            broadcastIEP = new IPEndPoint(targetIP,targetPort);
            client = new UdpClient(targetPort);
            isOn=true;
            Task.Run(loopReceive);
        }

        public static void On(int targetPort=PortConfig.BROADCAST_RECEIVE)
        {
            On(IPAddress.Broadcast,targetPort);
        }

        public static void Off()
        {
            if(!isOn)return;
            if(AndroidWifi.NotNull()) AndroidWifi.I.LockRelease();
            isOn = false;
            client.Close();
        }

        public async static void Send(byte[] buffer)
        {
            try
            {
                await client.SendAsync(buffer,buffer.Length,broadcastIEP);
            }
            catch (System.Exception){}
        }

        private async static void loopReceive()
        {
            while (isOn)
            {
                UdpReceiveResult result;
                try
                {
                    result = await client.ReceiveAsync();
                }
                catch (System.Exception ex)
                {
                    if(isOn)
                    {
                        UnityEngine.Debug.LogError(ex.Message);
                    }
                    continue;
                }
                if(OnReceive!=null)OnReceive(result);
            }
        }

    }
}