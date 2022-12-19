using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FGUFW;
using FGUFW.GamePlay;
using UnityEngine;

namespace Book
{
    public class BookShelfInput : IPart
    {
        public List<IPart> SubParts{get;set;}
        private BookShelfPanelComps _panelComps;

        public Task OnCreating(IPart parent)
        {
            _panelComps = GameObject.Find("BookShelfPanel").GetComponent<BookShelfPanelComps>();
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
            _panelComps.ImportBtn.onClick.AddListener(onClickImport);
            _panelComps.DeleteBtn.onClick.AddListener(onClickDelete);
            _panelComps.CancelDeleteBtn.onClick.AddListener(onClickCancelDelete);

            var records = BookPlay.I.GetPart<ConfigRecord>().Record.Items;
            _panelComps.ListRoot.Foreach<BookShelfPanelItemComps,BookItemRecord>(records,(comp,data)=>
            {
                comp.Btn.onClick.AddListener(()=>
                {
                    if(BookPlay.I.GetPart<BookShelf>().ItemClickMode==BookShelf.ClickItemMode.Read)
                    {
                        BookPlay.I.Messenger.Broadcast(BookMsgId.OnClickBookRecord,data);
                    }
                    else
                    {
                        var index = records.FindIndex(d=>d.FilePath==data.FilePath);
                        records.RemoveAt(index);
                        BookPlay.I.GetPart<ConfigRecord>().SaveRecord();
                        BookPlay.I.Messenger.Broadcast(BookMsgId.ResetBookShelf);
                    }
                });
            });
        }



        private void removeListener()
        {
            _panelComps.ImportBtn.onClick.RemoveListener(onClickImport);
            _panelComps.DeleteBtn.onClick.RemoveListener(onClickDelete);
            _panelComps.CancelDeleteBtn.onClick.RemoveListener(onClickCancelDelete);

            var records = BookPlay.I.GetPart<ConfigRecord>().Record.Items;
            _panelComps.ListRoot.Foreach<BookShelfPanelItemComps,BookItemRecord>(records,(comp,data)=>
            {
                comp.Btn.onClick.RemoveAllListeners();
            });
        }

        private void onClickCancelDelete()
        {
            BookPlay.I.GetPart<BookShelf>().ItemClickMode = BookShelf.ClickItemMode.Read;

            _panelComps.ImportBtn.gameObject.SetActive(true);
            _panelComps.DeleteBtn.gameObject.SetActive(true);
            _panelComps.CancelDeleteBtn.gameObject.SetActive(false);
        }

        private void onClickDelete()
        {
            BookPlay.I.GetPart<BookShelf>().ItemClickMode = BookShelf.ClickItemMode.Delete;

            _panelComps.ImportBtn.gameObject.SetActive(false);
            _panelComps.DeleteBtn.gameObject.SetActive(false);
            _panelComps.CancelDeleteBtn.gameObject.SetActive(true);
        }

        private void onClickImport()
        {
            BookPlay.I.Messenger.Broadcast(BookMsgId.ImportBegin);
        }
    }
}