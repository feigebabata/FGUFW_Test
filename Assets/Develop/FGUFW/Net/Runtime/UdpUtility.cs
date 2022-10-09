using System;
using System.Net;
using System.Net.Sockets;

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
            broadcastIEP = new IPEndPoint(IPAddress.Broadcast,PortConfig.BROADCAST_RECEIVE);
            sendClient = new UdpClient(PortConfig.BROADCAST_SEND);
            receiveClient = new UdpClient(PortConfig.BROADCAST_RECEIVE);
            isOn=true;
            loopReceive();
        }

        public static void Off()
        {
            if(!isOn)return;
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

        private async static void loopReceive()
        {
            while (isOn)
            {
                UdpReceiveResult result;
                try
                {
                    result = await receiveClient.ReceiveAsync();
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