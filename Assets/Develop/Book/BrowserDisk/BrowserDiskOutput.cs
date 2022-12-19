using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using FGUFW;
using FGUFW.GamePlay;
using UnityEngine.UI;

namespace Book
{
    public class BrowserDiskOutput : IPart,IAssetLoadable
    {
        private BrowserDiskPanelComps _panelComps;
        public List<IPart> SubParts{get;set;}

        public async Task OnCreating(IPart parent)
        {
            var panel = await this.InstantiateAsync("Book/BrowserDisk/BrowserDiskPanel.prefab",null);
            panel.name = "BrowserDiskPanel";
            _panelComps = panel.GetComponent<BrowserDiskPanelComps>();
            
            addListener();
        }

        public Task OnDestroying(IPart parent)
        {
            removeListener();
            return null;
        }

        private void addListener()
        {
            BookPlay.I.Messenger.Add(BookMsgId.ImportBegin,onImportBegin,10);
            BookPlay.I.Messenger.Add<string>(BookMsgId.ImportEnd,onImportEnd);
            BookPlay.I.Messenger.Add(BookMsgId.OnClickEscape,onClickEscape,10);
        }

        private void removeListener()
        {
            BookPlay.I.Messenger.Remove(BookMsgId.ImportBegin,onImportBegin);
            BookPlay.I.Messenger.Remove<string>(BookMsgId.ImportEnd,onImportEnd);
            BookPlay.I.Messenger.Remove(BookMsgId.OnClickEscape,onClickEscape);
        }

        private void onClickBookRecord(string obj)
        {
            throw new NotImplementedException();
        }

        private void onImportEnd(string obj)
        {
            _panelComps.gameObject.SetActive(false);
        }

        private void onImportBegin()
        {
            showList();
            _panelComps.gameObject.SetActive(true);
        }

        private void showList()
        {
            var diskPath = BookPlay.I.GetPart<ConfigRecord>().DiskPath;
            _panelComps.Title.text = diskPath.Path;
            var dirCount = diskPath.Directorys.Count;
            int length = diskPath.Directorys.Count + diskPath.Files.Count;
            _panelComps.ListRoot.For<Image>(length,(i,comp)=>
            {
                if(i<dirCount)
                {
                    var name = Path.GetFileName(diskPath.Directorys[i]);
                    comp.transform.GetChild(0).GetComponent<Text>().text = string.IsNullOrEmpty(name)?diskPath.Directorys[i]:name;
                    comp.color = _panelComps.DirectoryColor;
                }
                else
                {
                    comp.transform.GetChild(0).GetComponent<Text>().text = Path.GetFileName(diskPath.Files[i-dirCount]);
                    comp.color = _panelComps.FileColor;
                }
            });
        }

        private void onClickEscape()
        {
            if(_panelComps.gameObject.activeSelf)
            {
                _panelComps.gameObject.SetActive(false);
                BookPlay.I.Messenger.Abort(BookMsgId.OnClickEscape);
            }
        }
    }
}