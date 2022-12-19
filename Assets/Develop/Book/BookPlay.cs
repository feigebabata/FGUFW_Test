using System;
using System.Threading.Tasks;
using FGUFW;
using FGUFW.GamePlay;
using UnityEngine;

namespace Book
{
    public class BookPlay:Play<BookPlay>,IUpdate
    {
        public override Task OnCreating(IPart parent)
        {
            SubParts.Add(new ConfigRecord());
            SubParts.Add(new BookShelf());
            SubParts.Add(new BrowserDisk());
            SubParts.Add(new Page());
            
            return base.OnCreating(parent);
        }

        public void Update()
        {
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                Messenger.Broadcast(BookMsgId.OnClickEscape);
            }
        }
    }
}