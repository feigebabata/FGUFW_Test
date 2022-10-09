using System;

namespace FGUFW.Net
{
    public static class PackUtility
    {
        public const ushort PACK_HEAD_LENGTH = 16;
        public const ushort PACK_APPID_LENGTH = 2;
        public const ushort PACK_LEN_LENGTH = 2;
        public const ushort PACK_ROOM_LENGTH = 8;
        public const ushort PACK_CMD_LENGTH = 4;

        /// <summary>
        /// 数据包[ appid 2 | length 2 | roomid(时间戳) 8 |cmd 4| msgdata ]
        /// </summary>
        public static byte[] UdpPack(ushort appId,long roomId,uint cmd,byte[] buffer)
        {
            ushort bufferLength = (ushort)(buffer.Length+PACK_HEAD_LENGTH);
            byte[] sendBuffer = new byte[bufferLength];
            byte[] appIdBuffer = BitConverter.GetBytes(appId);
            byte[] roomIdBuffer = BitConverter.GetBytes(roomId);
            byte[] cmdBuffer = BitConverter.GetBytes(cmd);
            byte[] lengthBuffer = BitConverter.GetBytes(bufferLength);

            int index = 0,length=0;

            length = appIdBuffer.Length;
            Array.Copy(appIdBuffer,0,sendBuffer,index,length);
            index+=length;

            length = lengthBuffer.Length;
            Array.Copy(lengthBuffer,0,sendBuffer,index,length);
            index+=length;

            length = roomIdBuffer.Length;
            Array.Copy(roomIdBuffer,0,sendBuffer,index,length);
            index+=length;

            length = cmdBuffer.Length;
            Array.Copy(cmdBuffer,0,sendBuffer,index,length);
            index+=length;

            length = buffer.Length;
            Array.Copy(buffer,0,sendBuffer,index,length);

            return sendBuffer;
        }

        public static bool UdpUnpack(ref ushort appId,ref ushort length,ref long roomId,ref uint cmd,byte[] buffer,int index,int bufLen)
        {
            if(buffer!=null && bufLen<PACK_HEAD_LENGTH)
            {
                return false;
            }
            appId = BitConverter.ToUInt16(buffer,index);
            index += PACK_APPID_LENGTH;

            length = BitConverter.ToUInt16(buffer,index);
            index += PACK_LEN_LENGTH;

            roomId = BitConverter.ToInt64(buffer,index);
            index += PACK_ROOM_LENGTH;

            cmd = BitConverter.ToUInt32(buffer,index);
            index += PACK_CMD_LENGTH;
            
            return true;
        }

    }
}