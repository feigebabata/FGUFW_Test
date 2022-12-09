using System.Collections.Generic;
using System.Threading.Tasks;

namespace GamePlay
{
    public interface IPart
    {
        List<IPart> SubParts{get;set;}

        Task OnCreating(IPart parent);
        
        Task OnDestroying(IPart parent);
    }


    public interface IEnabled
    {
        bool Enable{get;}

        void OnEnable();

        void OnDisable();
    }

}