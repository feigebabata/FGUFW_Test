using System.Collections.Generic;
using System.Threading.Tasks;

namespace FGUFW.GamePlay
{
    public interface IPart
    {
        List<IPart> SubParts{get;set;}

        Task OnCreating(IPart parent);
        
        Task OnDestroying(IPart parent);
    }

}