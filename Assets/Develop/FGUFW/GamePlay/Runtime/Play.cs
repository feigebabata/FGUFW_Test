using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FGUFW;

namespace FGUFW.GamePlay
{
    public abstract class Play<T>:IPart where T:Play<T>
    {
        public static T I;
        public IOrderedMessenger<Enum> Messenger;

        public IList<IPart> SubParts {get;set;} = new List<IPart>();

        public virtual Task OnCreating(IPart parent)
        {
            I = (T)this;
            Messenger = new OrderedMessenger<Enum>();
            return null;
        }

        public virtual Task OnDestroying(IPart parent)
        {
            Messenger = null;
            I = null;
            return null;
        }

        


    }
}