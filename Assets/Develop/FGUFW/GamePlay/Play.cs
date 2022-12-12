using System.Collections.Generic;
using System.Threading.Tasks;

namespace GamePlay
{
    public abstract class Play<T>:IPart where T:Play<T>
    {
        public T I;

        public List<IPart> SubParts {get;set;} = new List<IPart>();

        public virtual Task OnCreating(IPart parent)
        {
            I = (T)this;
            return null;
        }

        public virtual Task OnDestroying(IPart parent)
        {
            I = null;
            return null;
        }

        


    }
}