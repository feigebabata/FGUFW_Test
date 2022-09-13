
using System;
using System.Collections;
using System.Collections.Generic;
using FGUFW;

namespace FGUFW.ECS
{
    public partial class World
    {
        

        public void Filter<T0>(Action<T0> callback)
        where T0:struct,IComponent
        {
            var t0_tVal = ComponentTypeHelper.GetTypeValue<T0>();

            if
            (
                !_compDict.ContainsKey(t0_tVal) || _compDict[t0_tVal].Count==0
            )return;

            Dictionary<int, T0> t0_dict = (Dictionary<int, T0>)_compDict[t0_tVal];

            ICollection<int> minCountEntityUIds = null;
            int minCount = int.MaxValue;

            if(t0_dict.Count<minCount)
            {
                minCount = t0_dict.Count;
                minCountEntityUIds = t0_dict.Keys;
            }

            foreach (var entityUId in minCountEntityUIds)
            {
                T0 t0_comp;
                if(!t0_dict.TryGetValue(entityUId,out t0_comp)) continue;

                callback(t0_comp);

            }
        }     


    }
}
