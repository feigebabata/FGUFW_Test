using System;
using System.Collections;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace FGUFW
{
    public static class FileHelper
    {

        public static Encoding GetEncoding(string path)
        {
            byte[] Unicode = new byte[] { 0xFF, 0xFE, 0x41 };
            byte[] UnicodeBIG = new byte[] { 0xFE, 0xFF, 0x00 };
            byte[] UTF8 = new byte[] { 0xEF, 0xBB, 0xBF }; //带BOM 
            Encoding reVal = Encoding.Default;

            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            BinaryReader r = new BinaryReader(fs, System.Text.Encoding.Default);
            int i;
            int.TryParse(fs.Length.ToString(), out i);
            byte[] ss = r.ReadBytes(i);

            if (IsUTF8Bytes(ss) || (ss[0] == 0xEF && ss[1] == 0xBB && ss[2] == 0xBF))
            {
                reVal = Encoding.UTF8;
            }
            else if (ss[0] == 0xFE && ss[1] == 0xFF && ss[2] == 0x00)
            {
                reVal = Encoding.BigEndianUnicode;
            }
            else if (ss[0] == 0xFF && ss[1] == 0xFE && ss[2] == 0x41)
            {
                reVal = Encoding.Unicode;
            }
            r.Close();
            return reVal;
        }

        /// <summary> 
        /// 判断是否是不带 BOM 的 UTF8 格式 
        /// </summary> 
        /// <param name=“data“></param> 
        /// <returns></returns> 
        private static bool IsUTF8Bytes(byte[] data)
        {
            int charByteCounter = 1; //计算当前正分析的字符应还有的字节数 
            byte curByte; //当前分析的字节. 
            for (int i = 0; i < data.Length; i++)
            {
                curByte = data[i];
                if (charByteCounter == 1)
                {
                    if (curByte >= 0x80)
                    {
                        //判断当前 
                        while (((curByte <<= 1) & 0x80) != 0)
                        {
                            charByteCounter++;
                        }
                        //标记位首位若为非0 则至少以2个1开始 如:110XXXXX...........1111110X 
                        if (charByteCounter == 1 || charByteCounter > 6)
                        {
                            return false;
                        }
                    }
                }
                else
                {
                    //若是UTF-8 此时第一位必须为1 
                    if ((curByte & 0xC0) != 0x80)
                    {
                        return false;
                    }
                    charByteCounter--;
                }
            }
            if (charByteCounter > 1)
            {
                throw new Exception("非预期的byte格式");
            }
            return true;
        }

        public static void LocalWrite(string localPath,byte[] data)
        {
            var path = Path.Combine(Application.persistentDataPath,localPath);
            var directoryPath = Path.GetDirectoryName(path);
            if(!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            File.WriteAllBytes(path,data);

        }

        public static void LocalWrite(string localPath,string text)
        {
            var path = Path.Combine(Application.persistentDataPath,localPath);
            var directoryPath = Path.GetDirectoryName(path);
            if(!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            File.WriteAllText(path,text);

        }

        public static byte[] LocaRead(string localPath)
        {
            var path = Path.Combine(Application.persistentDataPath,localPath);
            
            if(File.Exists(path))
            {
                return File.ReadAllBytes(path);
            }

            return default;

        }

        public static string LocaReadText(string localPath)
        {
            var path = Path.Combine(Application.persistentDataPath,localPath);
            
            if(File.Exists(path))
            {
                return File.ReadAllText(path);
            }

            return default;

        }

        public static IEnumerator LoadStreaming(string localPath,Action<string,DownloadHandler> callback)
        {
            var path = Path.Combine(Application.streamingAssetsPath,localPath);
            var uri = new Uri(path);
            using (var uwr = UnityWebRequest.Get(uri))
            {
                uwr.downloadHandler = new DownloadHandlerBuffer();
                yield return uwr.SendWebRequest();
                callback(uwr.error,uwr.downloadHandler);
            }
        }
        
    }
}