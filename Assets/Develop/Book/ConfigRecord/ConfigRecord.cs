using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using FGUFW;
using FGUFW.GamePlay;
using UnityEngine;

namespace Book
{
    public class ConfigRecord : IPart
    {
        public List<IPart> SubParts{get;set;}
        private const string RECORD_KEY = "RECORD_KEY";

        public BookRecord Record{get;private set;}
        public DiskPath DiskPath{get;private set;}

        public Task OnCreating(IPart parent)
        {
            loadRecord();
            return null;
        }

        public Task OnDestroying(IPart parent)
        {
            SaveRecord();
            return null;
        }

        private void loadRecord()
        {
            if(PlayerPrefs.HasKey(RECORD_KEY))
            {
                Record = JsonUtility.FromJson<BookRecord>(PlayerPrefs.GetString(RECORD_KEY,null));
            }
            else
            {
                Record = new BookRecord();
            }
            DiskPath = new DiskPath(Record.DiskPath);
        }

        public void SaveRecord()
        {
            Record.DiskPath = DiskPath.Path;
            PlayerPrefs.SetString(RECORD_KEY,JsonUtility.ToJson(Record));
        }
    }

    [System.Serializable]
    public class BookRecord
    {
        public string DiskPath;
        public List<BookItemRecord> Items = new List<BookItemRecord>();
    }

    [System.Serializable]
    public class BookItemRecord
    {
        public string Name;
        public string FilePath;
        public float Progress;
        public int ChapterIndex;
        public string ChapterName;
        public float ChapterViewOffset;
        public bool UseGB2312;

        public BookItemRecord(string filePath)
        {
            FilePath = filePath;
            Name = Path.GetFileName(filePath).Replace(Path.GetExtension(filePath),"");
        }
    }

}