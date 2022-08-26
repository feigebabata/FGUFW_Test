using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using LitJson;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

namespace FGUFW.MultiLanguage
{
    public static class TranslateOnline
    {
        const string FILE_PATH = "/Develop/CSV/MultiLanguageConfig.csv";
        static bool translating = false;

        [UnityEditor.MenuItem("多语言/在线翻译")]
        private static async void translateOnline()
        {
            if(translating)
            {
                Debug.LogError("任务进行中,请等待或查看Background Tasks");
                return;
            }
            translating = true;
            int progressId = Progress.Start("在线翻译多语言配置表");
            var filePath = Application.dataPath+FILE_PATH;
            var lines = File.ReadAllText(filePath).ToCsvLines();
            var lineCount = lines.Length;
            var itemCount = lines[0].Split(',').Length;
            string[][] data = new string[lineCount][];
            for (int i = 0; i < lineCount; i++)
            {
                var items = lines[i].Split(',');
                data[i] = new string[itemCount];
                for (int j = 0; j < items.Length; j++)
                {
                    data[i][j]=items[j];
                }
            }

            int totalCount = (itemCount-2)*(lineCount-3);
            int currentIndex = 0;

            var sl = data[2][1];
            for (int i = 2; i < itemCount; i++)
            {
                var tl = data[2][i];
                for (int j = 3; j < lineCount; j++)
                {
                    currentIndex++;
                    Progress.Report(progressId, currentIndex / (float)totalCount,$"在线翻译多语言:{currentIndex}/{totalCount}");

                    if(!string.IsNullOrEmpty(data[j][i]))continue;
                    var q = data[j][1];
                    q = Uri.EscapeUriString(q);
                    var url = $"https://translate.googleapis.com/translate_a/single?client=gtx&dt=t&sl={sl}&tl={tl}&q={q}";
                    UnityWebRequest uwr = new UnityWebRequest(url);

                    uwr.downloadHandler = new DownloadHandlerBuffer();
                    await uwr.RequestAsync();
                    // Debug.Log(uwr.downloadHandler.text);
                    
                    var newText = "";
                    try
                    {
                        var jsonData = JsonMapper.ToObject(uwr.downloadHandler.text);
                        foreach (JsonData item in jsonData[0])
                        {
                            newText = newText+item[0].ToString();
                        }
                        // newText = jsonData[0][0][0].ToString();
                        newText=newText.Replace(',','，');
                    }
                    catch
                    {
                        Debug.LogError($"{q}\n{uwr.downloadHandler.text}\n{uwr.error}");
                    }
                    if(newText.Contains("\n"))
                    {
                        char[] chars = newText.ToCharArray();
                        chars[0]='\"';
                        chars[chars.Length-1]='\"';
                        newText = new string(chars);
                    }
                    data[j][i] = newText;
                    // yield return new WaitForSeconds(1);
                }
            }
            for (int i = 0; i < lineCount; i++)
            {
                lines[i] = string.Join(",",data[i]);
            }
            File.WriteAllLines(filePath,lines,new UTF8Encoding(true));
            translating = false;
            Progress.Remove(progressId);
        }

    }
}
