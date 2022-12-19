using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FGUFW;
using FGUFW.GamePlay;

namespace Book
{
    public class BookShelfOutput : IPart,IAssetLoadable
    {
        private BookShelfPanelComps _panelComps;
        public List<IPart> SubParts{get;set;}

        public async Task OnCreating(IPart parent)
        {
            var panel = await this.InstantiateAsync("Book/BookShelf/BookShelfPanel.prefab",null);
            panel.name = "BookShelfPanel";
            _panelComps = panel.GetComponent<BookShelfPanelComps>();
            showRecordList();
            addListener();
        }

        public Task OnDestroying(IPart parent)
        {
            removeListener();
            return null;
        }

        private void addListener()
        {
            BookPlay.I.Messenger.Add<string>(BookMsgId.ImportEnd,onImprotBook);
            BookPlay.I.Messenger.Add<BookItemRecord>(BookMsgId.OnClickBookRecord,onClickBookRecord,-1);
            BookPlay.I.Messenger.Add(BookMsgId.BackBookShelf,backBookShelf);
            BookPlay.I.Messenger.Add(BookMsgId.ResetBookShelf,resetBookShelf);
        }

        private void removeListener()
        {
            BookPlay.I.Messenger.Remove<string>(BookMsgId.ImportEnd,onImprotBook);
            BookPlay.I.Messenger.Remove<BookItemRecord>(BookMsgId.OnClickBookRecord,onClickBookRecord);
            BookPlay.I.Messenger.Remove(BookMsgId.BackBookShelf,backBookShelf);
            BookPlay.I.Messenger.Remove(BookMsgId.ResetBookShelf,resetBookShelf);
        }

        private void resetBookShelf()
        {
            showRecordList();
        }

        private void backBookShelf()
        {
            _panelComps.gameObject.SetActive(true);
        }

        private void onClickBookRecord(BookItemRecord obj)
        {
            _panelComps.gameObject.SetActive(false);
        }

        private void onImprotBook(string obj)
        {
            showRecordList();
        }

        private void showRecordList()
        {
            var records = BookPlay.I.GetPart<ConfigRecord>().Record.Items;
            _panelComps.ListRoot.Foreach<BookShelfPanelItemComps,BookItemRecord>(records,(comp,data)=>
            {
                comp.Title.text = data.Name;
                comp.Info.text = $"{(int)(data.Progress*100)}% | {data.ChapterName}";
            });
        }
    }
}