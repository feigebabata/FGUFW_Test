using System.Collections.Generic;
using System.Threading.Tasks;

namespace FGUFW.GamePlay
{
    public interface IPart
    {
        IList<IPart> SubParts{get;set;}

        /// <summary>
        /// 回调函数 不要主动调用
        /// </summary>
        Task OnCreating(IPart parent);
        

        /// <summary>
        /// 回调函数 不要主动调用
        /// </summary>
        Task OnDestroying(IPart parent);
    }

}