using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FGUFW;
using FGUFW.GamePlay;
using UnityEngine;
using UnityEngine.UI;

namespace Book
{
    public class BrowserDiskInput : IPart
    {
        public List<IPart> SubParts{get;set;}
        private BrowserDiskPanelComps _panelComps;

        public Task OnCreating(IPart parent)
        {
            _panelComps = GameObject.Find("BrowserDiskPanel").GetComponent<BrowserDiskPanelComps>();
            _panelComps.gameObject.SetActive(false);
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
            BookPlay.I.Messenger.Add(BookMsgId.ImportBegin,onImportBegin);
            BookPlay.I.Messenger.Add<string>(BookMsgId.ImportEnd,onImportEnd);
            
            _panelComps.BackBtn.onClick.AddListener(onClickBack);
            _panelComps.CloseBtn.onClick.AddListener(onClickClose);
        }

        private void removeListener()
        {
            BookPlay.I.Messenger.Remove(BookMsgId.ImportBegin,onImportBegin);
            BookPlay.I.Messenger.Remove<string>(BookMsgId.ImportEnd,onImportEnd);
            
            _panelComps.BackBtn.onClick.RemoveListener(onClickBack);
            _panelComps.CloseBtn.onClick.RemoveListener(onClickClose);
        }

        private void onClickClose()
        {
            BookPlay.I.Messenger.Broadcast<string>(BookMsgId.ImportEnd,null);
        }

        private void onClickBack()
        {
            BookPlay.I.GetPart<ConfigRecord>().DiskPath.Back();
            BookPlay.I.GetPart<ConfigRecord>().SaveRecord();
            BookPlay.I.Messenger.Broadcast(BookMsgId.ImportBegin);
        }

        private void onImportEnd(string obj)
        {
            var diskPath = BookPlay.I.GetPart<ConfigRecord>().DiskPath;
            var dirCount = diskPath.Directorys.Count;
            int length = diskPath.Directorys.Count + diskPath.Files.Count;
            _panelComps.ListRoot.For<Button>(length,(i,comp)=>
            {
                comp.onClick.RemoveAllListeners();
            });
        }

        private void onImportBegin()
        {
            var diskPath = BookPlay.I.GetPart<ConfigRecord>().DiskPath;
            var dirCount = diskPath.Directorys.Count;
            int length = diskPath.Directorys.Count + diskPath.Files.Count;
            _panelComps.ListRoot.For<Button>(length,(i,comp)=>
            {
                comp.onClick.RemoveAllListeners();
                if(i<dirCount)
                {
                    comp.onClick.AddListener(()=>{onClickDir(diskPath.Directorys[i]);});
                }
                else
                {
                    comp.onClick.AddListener(()=>{onClickFile(diskPath.Files[i-dirCount]);});
                }
            });
        }

        private void onClickDir(string v)
        {
            BookPlay.I.GetPart<ConfigRecord>().DiskPath.SetPath(v);
            BookPlay.I.GetPart<ConfigRecord>().SaveRecord();
            BookPlay.I.Messenger.Broadcast(BookMsgId.ImportBegin);
        }

        private void onClickFile(string v)
        {
            BookPlay.I.Messenger.Broadcast(BookMsgId.ImportEnd,v);
        }

    }
}