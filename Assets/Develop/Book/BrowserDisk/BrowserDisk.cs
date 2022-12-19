using System.Collections.Generic;
using System.Threading.Tasks;
using FGUFW;
using FGUFW.GamePlay;

namespace Book
{
    public class BrowserDisk : IPart
    {
        public List<IPart> SubParts{get;set;} = new List<IPart>();

        public Task OnCreating(IPart parent)
        {
            SubParts.Add(new BrowserDiskOutput());
            SubParts.Add(new BrowserDiskInput());

            return null;
        }

        public Task OnDestroying(IPart parent)
        {
            return null;
        }
    }
}