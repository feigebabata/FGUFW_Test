using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using FGUFW;
using FGUFW.GamePlay;
using UnityEngine;

namespace Book
{
    public class Page : IPart,IAssetLoadable
    {
        public List<IPart> SubParts{get;set;}=new List<IPart>();

        private PagePanelComps _panelComps;
        public BookItemRecord Record;
        public string[] Lines;
        public Vector2Int[] ChapterIndexs;

        public async Task OnCreating(IPart parent)
        {
            var panel = await this.InstantiateAsync("Book/Page/PagePanel.prefab",null);
            panel.name = "PagePanel";
            _panelComps = panel.GetComponent<PagePanelComps>();
            _panelComps.gameObject.SetActive(false);

            SubParts.Add(new PageSetting());
            SubParts.Add(new PageList());

            addListener();
        }

        public Task OnDestroying(IPart parent)
        {
            removeListener();
            return null;
        }

        private void addListener()
        {
            BookPlay.I.Messenger.Add<BookItemRecord>(BookMsgId.OnClickBookRecord,onClickBookRecord);
            BookPlay.I.Messenger.Add(BookMsgId.OnClickEscape,onClickEscape,5);

            _panelComps.SettingBtn.onClick.AddListener(onClickSetting);
        }

        private void removeListener()
        {
            BookPlay.I.Messenger.Remove<BookItemRecord>(BookMsgId.OnClickBookRecord,onClickBookRecord);
            BookPlay.I.Messenger.Remove(BookMsgId.OnClickEscape,onClickEscape);

            _panelComps.SettingBtn.onClick.RemoveListener(onClickSetting);
        }

        private void onClickEscape()
        {
            if(_panelComps.gameObject.activeSelf)
            {
                _panelComps.gameObject.SetActive(false);
                BookPlay.I.Messenger.Abort(BookMsgId.OnClickEscape);
                BookPlay.I.Messenger.Broadcast(BookMsgId.BackBookShelf);

                Record.ChapterViewOffset = _panelComps.View.anchoredPosition.y;
                BookPlay.I.GetPart<ConfigRecord>().SaveRecord();
            }
        }

        private void onClickSetting()
        {
            BookPlay.I.Messenger.Broadcast(BookMsgId.ShowPageSetting);
        }

        private void onClickBookRecord(BookItemRecord obj)
        {
            Record = obj;
            _panelComps.gameObject.SetActive(true);
            Lines = NovelTextHelper.FormatLines(File.ReadAllLines(obj.FilePath,obj.UseGB2312?NovelTextHelper.GB2312:Encoding.UTF8));
            ChapterIndexs = NovelTextHelper.SplitChapter(Lines);

            ShowPage(obj.ChapterIndex,obj.ChapterViewOffset);
        }

        public void ShowPage(int index,float offset=0)
        {
            if(index<0)return;
            if(index>=ChapterIndexs.Length)index=ChapterIndexs.Length-1;
            
            _panelComps.Title.text = Lines[ChapterIndexs[index].x];
            _panelComps.TextComp.text = NovelTextHelper.GetChapter(Lines,ChapterIndexs[index]);
            var anchoredPosition = _panelComps.View.anchoredPosition;
            anchoredPosition.y = offset;
            _panelComps.View.anchoredPosition = anchoredPosition;

            Record.ChapterIndex = index;
            Record.ChapterName = Lines[ChapterIndexs[index].x];
            Record.ChapterViewOffset = 0;
            Record.Progress = index/(float)ChapterIndexs.Length;

            BookPlay.I.GetPart<ConfigRecord>().SaveRecord();
        }

        internal void SetFontSize(int arg0)
        {
            _panelComps.TextComp.fontSize =arg0;
        }

        internal int GetFontSize()
        {
            return _panelComps.TextComp.fontSize;
        }
        
    }
}