using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FGUFW;
using FGUFW.GamePlay;
using UnityEngine;

namespace Book
{
    public class PageSetting : IPart,IAssetLoadable
    {
        public List<IPart> SubParts{get;set;}
        private PageSettingPanelComps _panelComps;

        public async Task OnCreating(IPart parent)
        {
            var panel = await this.InstantiateAsync("Book/Page/PageSettingPanel.prefab",null);
            panel.name = "PageSettingPanel";
            _panelComps = panel.GetComponent<PageSettingPanelComps>();
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
            BookPlay.I.Messenger.Add(BookMsgId.ShowPageSetting,onShowPageSetting);
            BookPlay.I.Messenger.Add(BookMsgId.OnClickEscape,onClickEscape,10);

            _panelComps.Next.onClick.AddListener(onClickNext);
            _panelComps.Previous.onClick.AddListener(onClickPrevious);
            _panelComps.List.onClick.AddListener(onClickList);
            _panelComps.Close.onClick.AddListener(onClickClose);
            _panelComps.IndexRate.onValueChanged.AddListener(onValueChangedIndex);
            _panelComps.FontSize.onValueChanged.AddListener(onValueChangedFontSize);
        }

        private void removeListener()
        {
            BookPlay.I.Messenger.Remove(BookMsgId.ShowPageSetting,onShowPageSetting);
            BookPlay.I.Messenger.Remove(BookMsgId.OnClickEscape,onClickEscape);

            _panelComps.Next.onClick.RemoveListener(onClickNext);
            _panelComps.Previous.onClick.RemoveListener(onClickPrevious);
            _panelComps.List.onClick.RemoveListener(onClickList);
            _panelComps.Close.onClick.RemoveListener(onClickClose);
            _panelComps.IndexRate.onValueChanged.RemoveListener(onValueChangedIndex);
            _panelComps.FontSize.onValueChanged.RemoveListener(onValueChangedFontSize);
        }

        private void onClickEscape()
        {
            if(_panelComps.gameObject.activeSelf)
            {
                BookPlay.I.Messenger.Abort(BookMsgId.OnClickEscape);
                onClickClose();
            }
        }

        private void onValueChangedFontSize(float arg0)
        {
            BookPlay.I.GetPart<Page>().SetFontSize((int)arg0);
        }

        private void onValueChangedIndex(float arg0)
        {
            BookPlay.I.GetPart<Page>().ShowPage((int)arg0);
        }

        private void onClickClose()
        {
            _panelComps.gameObject.SetActive(false);
        }

        private void onClickList()
        {
            BookPlay.I.Messenger.Broadcast(BookMsgId.ShowPageList);
            _panelComps.gameObject.SetActive(false);
        }

        private void onClickPrevious()
        {
            var record = BookPlay.I.GetPart<Page>().Record;
            BookPlay.I.GetPart<Page>().ShowPage(record.ChapterIndex-1);
            _panelComps.gameObject.SetActive(false);
        }

        private void onClickNext()
        {
            var record = BookPlay.I.GetPart<Page>().Record;
            BookPlay.I.GetPart<Page>().ShowPage(record.ChapterIndex+1);
            _panelComps.gameObject.SetActive(false);
        }

        private void onShowPageSetting()
        {
            _panelComps.gameObject.SetActive(true);

            var page = BookPlay.I.GetPart<Page>();
            _panelComps.IndexRate.maxValue = page.ChapterIndexs.Length;
            _panelComps.IndexRate.value = page.Record.ChapterIndex;
            _panelComps.FontSize.value = page.GetFontSize();
        }
    }
}