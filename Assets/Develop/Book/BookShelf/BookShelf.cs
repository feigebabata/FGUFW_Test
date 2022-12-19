using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FGUFW;
using FGUFW.GamePlay;

namespace Book
{
    public class BookShelf : IPart
    {
        public ClickItemMode ItemClickMode = ClickItemMode.Read;
        public List<IPart> SubParts{get;set;} = new List<IPart>();

        public Task OnCreating(IPart parent)
        {
            SubParts.Add(new BookShelfOutput());
            SubParts.Add(new BookShelfInput());

            addListener();
            return null;
        }

        public Task OnDestroying(IPart parent)
        {
            removeListener();
            return null;
        }

        private void addListener()
        {
            BookPlay.I.Messenger.Add<string>(BookMsgId.ImportEnd,onImportEnd);
            BookPlay.I.Messenger.Add(BookMsgId.OnClickEscape,onClickEscape);
        }

        private void removeListener()
        {
            BookPlay.I.Messenger.Remove<string>(BookMsgId.ImportEnd,onImportEnd);
            BookPlay.I.Messenger.Remove(BookMsgId.OnClickEscape,onClickEscape);
        }

        private void onClickEscape()
        {
            UnityEngine.Application.Quit();
        }

        private void onImportEnd(string obj)
        {
            var record = new BookItemRecord(obj);
            BookPlay.I.GetPart<ConfigRecord>().Record.Items.Add(record);
            BookPlay.I.GetPart<ConfigRecord>().SaveRecord();
        }

        public enum ClickItemMode
        {
            Read,Delete
        }

    }
}