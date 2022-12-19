using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FGUFW;
using FGUFW.GamePlay;
using UnityEngine;
using UnityEngine.UI;

namespace Book
{
    public class PageList : IPart,IAssetLoadable
    {
        public List<IPart> SubParts{get;set;}
        private PageListPanelComps _panelComps;

        public async Task OnCreating(IPart parent)
        {
            var panel = await this.InstantiateAsync("Book/Page/PageListPanel.prefab",null);
            panel.name = "PageListPanel";
            _panelComps = panel.GetComponent<PageListPanelComps>();
            _panelComps.gameObject.SetActive(false);
            addListener();
        }

        public Task OnDestroying(IPart parent)
        {
            removeListener();
            return null;
        }

        private void addListener()
        {
            _panelComps.Close.onClick.AddListener(onClickClose);

            BookPlay.I.Messenger.Add(BookMsgId.ShowPageList,onShowPageList);
            BookPlay.I.Messenger.Add(BookMsgId.OnClickEscape,onClickEscape,10);

        }

        private void removeListener()
        {
            _panelComps.Close.onClick.RemoveListener(onClickClose);

            BookPlay.I.Messenger.Remove(BookMsgId.ShowPageList,onShowPageList);
            BookPlay.I.Messenger.Remove(BookMsgId.OnClickEscape,onClickEscape);
        }

        private void onClickEscape()
        {
            if(_panelComps.gameObject.activeSelf)
            {
                BookPlay.I.Messenger.Abort(BookMsgId.OnClickEscape);
                onClickClose();
            }
        }

        private void onShowPageList()
        {
            _panelComps.gameObject.SetActive(true);
            var page = BookPlay.I.GetPart<Page>();
            _panelComps.List.Init(page.ChapterIndexs.Length,page.Record.ChapterIndex,onResetItem);
        }

        private void onResetItem(int arg1, Transform arg2)
        {
            var page = BookPlay.I.GetPart<Page>();
            arg2.transform.GetChild(0).GetComponent<Text>().text = page.Lines[page.ChapterIndexs[arg1].x];
            arg2.GetComponent<Button>().onClick.RemoveAllListeners();
            arg2.GetComponent<Button>().onClick.AddListener(()=>{onClickItem(arg1);});
        }

        private void onClickItem(int index)
        {
            BookPlay.I.GetPart<Page>().ShowPage(index);
            onClickClose();
        }

        private void onClickClose()
        {
            _panelComps.gameObject.SetActive(false);
        }
    }
}