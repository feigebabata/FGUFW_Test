
using System.Collections.Generic;

namespace FGUFW.ECS
{
    
    public delegate void WorldFilterDelegate_R1<T0>(ref T0 t0);
    public delegate void WorldFilterDelegate_R2<T0,T1>(ref T0 t0,ref T1 t1);
    public delegate void WorldFilterDelegate_R3<T0,T1,T2>(ref T0 t0,ref T1 t1,ref T2 t2);
    public delegate void WorldFilterDelegate_R4<T0,T1,T2,T3>(ref T0 t0,ref T1 t1,ref T2 t2,ref T3 t3);
    public delegate void WorldFilterDelegate_R5<T0,T1,T2,T3,T4>(ref T0 t0,ref T1 t1,ref T2 t2,ref T3 t3,ref T4 t4);
    public delegate void WorldFilterDelegate_R6<T0,T1,T2,T3,T4,T5>(ref T0 t0,ref T1 t1,ref T2 t2,ref T3 t3,ref T4 t4,ref T5 t5);
    public delegate void WorldFilterDelegate_R7<T0,T1,T2,T3,T4,T5,T6>(ref T0 t0,ref T1 t1,ref T2 t2,ref T3 t3,ref T4 t4,ref T5 t5,ref T6 t6);
    public delegate void WorldFilterDelegate_R8<T0,T1,T2,T3,T4,T5,T6,T7>(ref T0 t0,ref T1 t1,ref T2 t2,ref T3 t3,ref T4 t4,ref T5 t5,ref T6 t6,ref T7 t7);

    public partial class World
    {
        

        public void Filter<T0>(WorldFilterDelegate_R1<T0> callback)
        where T0:struct,IComponent
        {
            var t0_tVal = ComponentTypeHelper.GetTypeValue<T0>();

            if
            (
                !_compDict.ContainsKey(t0_tVal) || _compDict[t0_tVal].Count==0
            )return;

            Dictionary<int, T0> t0_dict = _compDict[t0_tVal] as Dictionary<int, T0>;

            ICollection<int> minCountEntityUIds = null;
            int minCount = int.MaxValue;

            if(t0_dict.Count<minCount){minCount = t0_dict.Count;minCountEntityUIds = t0_dict.Keys;}

            foreach (var entityUId in minCountEntityUIds)
            {
                T0 t0_comp;if(!t0_dict.TryGetValue(entityUId,out t0_comp)) continue;

                callback(ref t0_comp);

                if(t0_comp.Dirty>0)t0_dict[entityUId]=t0_comp;
            }
        }     


        public void Filter<T0,T1>(WorldFilterDelegate_R2<T0,T1> callback)
        where T0:struct,IComponent
        where T1:struct,IComponent
        {
            var t0_tVal = ComponentTypeHelper.GetTypeValue<T0>();
            var t1_tVal = ComponentTypeHelper.GetTypeValue<T1>();

            if
            (
                !_compDict.ContainsKey(t0_tVal) || _compDict[t0_tVal].Count==0
                || !_compDict.ContainsKey(t1_tVal) || _compDict[t1_tVal].Count==0
            )return;

            Dictionary<int, T0> t0_dict = _compDict[t0_tVal] as Dictionary<int, T0>;
            Dictionary<int, T1> t1_dict = _compDict[t1_tVal] as Dictionary<int, T1>;

            ICollection<int> minCountEntityUIds = null;
            int minCount = int.MaxValue;

            if(t0_dict.Count<minCount){minCount = t0_dict.Count;minCountEntityUIds = t0_dict.Keys;}
            if(t1_dict.Count<minCount){minCount = t1_dict.Count;minCountEntityUIds = t1_dict.Keys;}

            foreach (var entityUId in minCountEntityUIds)
            {
                T0 t0_comp;if(!t0_dict.TryGetValue(entityUId,out t0_comp)) continue;
                T1 t1_comp;if(!t1_dict.TryGetValue(entityUId,out t1_comp)) continue;

                callback(ref t0_comp,ref t1_comp);

                if(t0_comp.Dirty>0)t0_dict[entityUId]=t0_comp;
                if(t1_comp.Dirty>0)t1_dict[entityUId]=t1_comp;
            }
        }     


        public void Filter<T0,T1,T2>(WorldFilterDelegate_R3<T0,T1,T2> callback)
        where T0:struct,IComponent
        where T1:struct,IComponent
        where T2:struct,IComponent
        {
            var t0_tVal = ComponentTypeHelper.GetTypeValue<T0>();
            var t1_tVal = ComponentTypeHelper.GetTypeValue<T1>();
            var t2_tVal = ComponentTypeHelper.GetTypeValue<T2>();

            if
            (
                !_compDict.ContainsKey(t0_tVal) || _compDict[t0_tVal].Count==0
                || !_compDict.ContainsKey(t1_tVal) || _compDict[t1_tVal].Count==0
                || !_compDict.ContainsKey(t2_tVal) || _compDict[t2_tVal].Count==0
            )return;

            Dictionary<int, T0> t0_dict = _compDict[t0_tVal] as Dictionary<int, T0>;
            Dictionary<int, T1> t1_dict = _compDict[t1_tVal] as Dictionary<int, T1>;
            Dictionary<int, T2> t2_dict = _compDict[t2_tVal] as Dictionary<int, T2>;

            ICollection<int> minCountEntityUIds = null;
            int minCount = int.MaxValue;

            if(t0_dict.Count<minCount){minCount = t0_dict.Count;minCountEntityUIds = t0_dict.Keys;}
            if(t1_dict.Count<minCount){minCount = t1_dict.Count;minCountEntityUIds = t1_dict.Keys;}
            if(t2_dict.Count<minCount){minCount = t2_dict.Count;minCountEntityUIds = t2_dict.Keys;}

            foreach (var entityUId in minCountEntityUIds)
            {
                T0 t0_comp;if(!t0_dict.TryGetValue(entityUId,out t0_comp)) continue;
                T1 t1_comp;if(!t1_dict.TryGetValue(entityUId,out t1_comp)) continue;
                T2 t2_comp;if(!t2_dict.TryGetValue(entityUId,out t2_comp)) continue;

                callback(ref t0_comp,ref t1_comp,ref t2_comp);

                if(t0_comp.Dirty>0)t0_dict[entityUId]=t0_comp;
                if(t1_comp.Dirty>0)t1_dict[entityUId]=t1_comp;
                if(t2_comp.Dirty>0)t2_dict[entityUId]=t2_comp;
            }
        }     


        public void Filter<T0,T1,T2,T3>(WorldFilterDelegate_R4<T0,T1,T2,T3> callback)
        where T0:struct,IComponent
        where T1:struct,IComponent
        where T2:struct,IComponent
        where T3:struct,IComponent
        {
            var t0_tVal = ComponentTypeHelper.GetTypeValue<T0>();
            var t1_tVal = ComponentTypeHelper.GetTypeValue<T1>();
            var t2_tVal = ComponentTypeHelper.GetTypeValue<T2>();
            var t3_tVal = ComponentTypeHelper.GetTypeValue<T3>();

            if
            (
                !_compDict.ContainsKey(t0_tVal) || _compDict[t0_tVal].Count==0
                || !_compDict.ContainsKey(t1_tVal) || _compDict[t1_tVal].Count==0
                || !_compDict.ContainsKey(t2_tVal) || _compDict[t2_tVal].Count==0
                || !_compDict.ContainsKey(t3_tVal) || _compDict[t3_tVal].Count==0
            )return;

            Dictionary<int, T0> t0_dict = _compDict[t0_tVal] as Dictionary<int, T0>;
            Dictionary<int, T1> t1_dict = _compDict[t1_tVal] as Dictionary<int, T1>;
            Dictionary<int, T2> t2_dict = _compDict[t2_tVal] as Dictionary<int, T2>;
            Dictionary<int, T3> t3_dict = _compDict[t3_tVal] as Dictionary<int, T3>;

            ICollection<int> minCountEntityUIds = null;
            int minCount = int.MaxValue;

            if(t0_dict.Count<minCount){minCount = t0_dict.Count;minCountEntityUIds = t0_dict.Keys;}
            if(t1_dict.Count<minCount){minCount = t1_dict.Count;minCountEntityUIds = t1_dict.Keys;}
            if(t2_dict.Count<minCount){minCount = t2_dict.Count;minCountEntityUIds = t2_dict.Keys;}
            if(t3_dict.Count<minCount){minCount = t3_dict.Count;minCountEntityUIds = t3_dict.Keys;}

            foreach (var entityUId in minCountEntityUIds)
            {
                T0 t0_comp;if(!t0_dict.TryGetValue(entityUId,out t0_comp)) continue;
                T1 t1_comp;if(!t1_dict.TryGetValue(entityUId,out t1_comp)) continue;
                T2 t2_comp;if(!t2_dict.TryGetValue(entityUId,out t2_comp)) continue;
                T3 t3_comp;if(!t3_dict.TryGetValue(entityUId,out t3_comp)) continue;

                callback(ref t0_comp,ref t1_comp,ref t2_comp,ref t3_comp);

                if(t0_comp.Dirty>0)t0_dict[entityUId]=t0_comp;
                if(t1_comp.Dirty>0)t1_dict[entityUId]=t1_comp;
                if(t2_comp.Dirty>0)t2_dict[entityUId]=t2_comp;
                if(t3_comp.Dirty>0)t3_dict[entityUId]=t3_comp;
            }
        }     


        public void Filter<T0,T1,T2,T3,T4>(WorldFilterDelegate_R5<T0,T1,T2,T3,T4> callback)
        where T0:struct,IComponent
        where T1:struct,IComponent
        where T2:struct,IComponent
        where T3:struct,IComponent
        where T4:struct,IComponent
        {
            var t0_tVal = ComponentTypeHelper.GetTypeValue<T0>();
            var t1_tVal = ComponentTypeHelper.GetTypeValue<T1>();
            var t2_tVal = ComponentTypeHelper.GetTypeValue<T2>();
            var t3_tVal = ComponentTypeHelper.GetTypeValue<T3>();
            var t4_tVal = ComponentTypeHelper.GetTypeValue<T4>();

            if
            (
                !_compDict.ContainsKey(t0_tVal) || _compDict[t0_tVal].Count==0
                || !_compDict.ContainsKey(t1_tVal) || _compDict[t1_tVal].Count==0
                || !_compDict.ContainsKey(t2_tVal) || _compDict[t2_tVal].Count==0
                || !_compDict.ContainsKey(t3_tVal) || _compDict[t3_tVal].Count==0
                || !_compDict.ContainsKey(t4_tVal) || _compDict[t4_tVal].Count==0
            )return;

            Dictionary<int, T0> t0_dict = _compDict[t0_tVal] as Dictionary<int, T0>;
            Dictionary<int, T1> t1_dict = _compDict[t1_tVal] as Dictionary<int, T1>;
            Dictionary<int, T2> t2_dict = _compDict[t2_tVal] as Dictionary<int, T2>;
            Dictionary<int, T3> t3_dict = _compDict[t3_tVal] as Dictionary<int, T3>;
            Dictionary<int, T4> t4_dict = _compDict[t4_tVal] as Dictionary<int, T4>;

            ICollection<int> minCountEntityUIds = null;
            int minCount = int.MaxValue;

            if(t0_dict.Count<minCount){minCount = t0_dict.Count;minCountEntityUIds = t0_dict.Keys;}
            if(t1_dict.Count<minCount){minCount = t1_dict.Count;minCountEntityUIds = t1_dict.Keys;}
            if(t2_dict.Count<minCount){minCount = t2_dict.Count;minCountEntityUIds = t2_dict.Keys;}
            if(t3_dict.Count<minCount){minCount = t3_dict.Count;minCountEntityUIds = t3_dict.Keys;}
            if(t4_dict.Count<minCount){minCount = t4_dict.Count;minCountEntityUIds = t4_dict.Keys;}

            foreach (var entityUId in minCountEntityUIds)
            {
                T0 t0_comp;if(!t0_dict.TryGetValue(entityUId,out t0_comp)) continue;
                T1 t1_comp;if(!t1_dict.TryGetValue(entityUId,out t1_comp)) continue;
                T2 t2_comp;if(!t2_dict.TryGetValue(entityUId,out t2_comp)) continue;
                T3 t3_comp;if(!t3_dict.TryGetValue(entityUId,out t3_comp)) continue;
                T4 t4_comp;if(!t4_dict.TryGetValue(entityUId,out t4_comp)) continue;

                callback(ref t0_comp,ref t1_comp,ref t2_comp,ref t3_comp,ref t4_comp);

                if(t0_comp.Dirty>0)t0_dict[entityUId]=t0_comp;
                if(t1_comp.Dirty>0)t1_dict[entityUId]=t1_comp;
                if(t2_comp.Dirty>0)t2_dict[entityUId]=t2_comp;
                if(t3_comp.Dirty>0)t3_dict[entityUId]=t3_comp;
                if(t4_comp.Dirty>0)t4_dict[entityUId]=t4_comp;
            }
        }     


        public void Filter<T0,T1,T2,T3,T4,T5>(WorldFilterDelegate_R6<T0,T1,T2,T3,T4,T5> callback)
        where T0:struct,IComponent
        where T1:struct,IComponent
        where T2:struct,IComponent
        where T3:struct,IComponent
        where T4:struct,IComponent
        where T5:struct,IComponent
        {
            var t0_tVal = ComponentTypeHelper.GetTypeValue<T0>();
            var t1_tVal = ComponentTypeHelper.GetTypeValue<T1>();
            var t2_tVal = ComponentTypeHelper.GetTypeValue<T2>();
            var t3_tVal = ComponentTypeHelper.GetTypeValue<T3>();
            var t4_tVal = ComponentTypeHelper.GetTypeValue<T4>();
            var t5_tVal = ComponentTypeHelper.GetTypeValue<T5>();

            if
            (
                !_compDict.ContainsKey(t0_tVal) || _compDict[t0_tVal].Count==0
                || !_compDict.ContainsKey(t1_tVal) || _compDict[t1_tVal].Count==0
                || !_compDict.ContainsKey(t2_tVal) || _compDict[t2_tVal].Count==0
                || !_compDict.ContainsKey(t3_tVal) || _compDict[t3_tVal].Count==0
                || !_compDict.ContainsKey(t4_tVal) || _compDict[t4_tVal].Count==0
                || !_compDict.ContainsKey(t5_tVal) || _compDict[t5_tVal].Count==0
            )return;

            Dictionary<int, T0> t0_dict = _compDict[t0_tVal] as Dictionary<int, T0>;
            Dictionary<int, T1> t1_dict = _compDict[t1_tVal] as Dictionary<int, T1>;
            Dictionary<int, T2> t2_dict = _compDict[t2_tVal] as Dictionary<int, T2>;
            Dictionary<int, T3> t3_dict = _compDict[t3_tVal] as Dictionary<int, T3>;
            Dictionary<int, T4> t4_dict = _compDict[t4_tVal] as Dictionary<int, T4>;
            Dictionary<int, T5> t5_dict = _compDict[t5_tVal] as Dictionary<int, T5>;

            ICollection<int> minCountEntityUIds = null;
            int minCount = int.MaxValue;

            if(t0_dict.Count<minCount){minCount = t0_dict.Count;minCountEntityUIds = t0_dict.Keys;}
            if(t1_dict.Count<minCount){minCount = t1_dict.Count;minCountEntityUIds = t1_dict.Keys;}
            if(t2_dict.Count<minCount){minCount = t2_dict.Count;minCountEntityUIds = t2_dict.Keys;}
            if(t3_dict.Count<minCount){minCount = t3_dict.Count;minCountEntityUIds = t3_dict.Keys;}
            if(t4_dict.Count<minCount){minCount = t4_dict.Count;minCountEntityUIds = t4_dict.Keys;}
            if(t5_dict.Count<minCount){minCount = t5_dict.Count;minCountEntityUIds = t5_dict.Keys;}

            foreach (var entityUId in minCountEntityUIds)
            {
                T0 t0_comp;if(!t0_dict.TryGetValue(entityUId,out t0_comp)) continue;
                T1 t1_comp;if(!t1_dict.TryGetValue(entityUId,out t1_comp)) continue;
                T2 t2_comp;if(!t2_dict.TryGetValue(entityUId,out t2_comp)) continue;
                T3 t3_comp;if(!t3_dict.TryGetValue(entityUId,out t3_comp)) continue;
                T4 t4_comp;if(!t4_dict.TryGetValue(entityUId,out t4_comp)) continue;
                T5 t5_comp;if(!t5_dict.TryGetValue(entityUId,out t5_comp)) continue;

                callback(ref t0_comp,ref t1_comp,ref t2_comp,ref t3_comp,ref t4_comp,ref t5_comp);

                if(t0_comp.Dirty>0)t0_dict[entityUId]=t0_comp;
                if(t1_comp.Dirty>0)t1_dict[entityUId]=t1_comp;
                if(t2_comp.Dirty>0)t2_dict[entityUId]=t2_comp;
                if(t3_comp.Dirty>0)t3_dict[entityUId]=t3_comp;
                if(t4_comp.Dirty>0)t4_dict[entityUId]=t4_comp;
                if(t5_comp.Dirty>0)t5_dict[entityUId]=t5_comp;
            }
        }     


        public void Filter<T0,T1,T2,T3,T4,T5,T6>(WorldFilterDelegate_R7<T0,T1,T2,T3,T4,T5,T6> callback)
        where T0:struct,IComponent
        where T1:struct,IComponent
        where T2:struct,IComponent
        where T3:struct,IComponent
        where T4:struct,IComponent
        where T5:struct,IComponent
        where T6:struct,IComponent
        {
            var t0_tVal = ComponentTypeHelper.GetTypeValue<T0>();
            var t1_tVal = ComponentTypeHelper.GetTypeValue<T1>();
            var t2_tVal = ComponentTypeHelper.GetTypeValue<T2>();
            var t3_tVal = ComponentTypeHelper.GetTypeValue<T3>();
            var t4_tVal = ComponentTypeHelper.GetTypeValue<T4>();
            var t5_tVal = ComponentTypeHelper.GetTypeValue<T5>();
            var t6_tVal = ComponentTypeHelper.GetTypeValue<T6>();

            if
            (
                !_compDict.ContainsKey(t0_tVal) || _compDict[t0_tVal].Count==0
                || !_compDict.ContainsKey(t1_tVal) || _compDict[t1_tVal].Count==0
                || !_compDict.ContainsKey(t2_tVal) || _compDict[t2_tVal].Count==0
                || !_compDict.ContainsKey(t3_tVal) || _compDict[t3_tVal].Count==0
                || !_compDict.ContainsKey(t4_tVal) || _compDict[t4_tVal].Count==0
                || !_compDict.ContainsKey(t5_tVal) || _compDict[t5_tVal].Count==0
                || !_compDict.ContainsKey(t6_tVal) || _compDict[t6_tVal].Count==0
            )return;

            Dictionary<int, T0> t0_dict = _compDict[t0_tVal] as Dictionary<int, T0>;
            Dictionary<int, T1> t1_dict = _compDict[t1_tVal] as Dictionary<int, T1>;
            Dictionary<int, T2> t2_dict = _compDict[t2_tVal] as Dictionary<int, T2>;
            Dictionary<int, T3> t3_dict = _compDict[t3_tVal] as Dictionary<int, T3>;
            Dictionary<int, T4> t4_dict = _compDict[t4_tVal] as Dictionary<int, T4>;
            Dictionary<int, T5> t5_dict = _compDict[t5_tVal] as Dictionary<int, T5>;
            Dictionary<int, T6> t6_dict = _compDict[t6_tVal] as Dictionary<int, T6>;

            ICollection<int> minCountEntityUIds = null;
            int minCount = int.MaxValue;

            if(t0_dict.Count<minCount){minCount = t0_dict.Count;minCountEntityUIds = t0_dict.Keys;}
            if(t1_dict.Count<minCount){minCount = t1_dict.Count;minCountEntityUIds = t1_dict.Keys;}
            if(t2_dict.Count<minCount){minCount = t2_dict.Count;minCountEntityUIds = t2_dict.Keys;}
            if(t3_dict.Count<minCount){minCount = t3_dict.Count;minCountEntityUIds = t3_dict.Keys;}
            if(t4_dict.Count<minCount){minCount = t4_dict.Count;minCountEntityUIds = t4_dict.Keys;}
            if(t5_dict.Count<minCount){minCount = t5_dict.Count;minCountEntityUIds = t5_dict.Keys;}
            if(t6_dict.Count<minCount){minCount = t6_dict.Count;minCountEntityUIds = t6_dict.Keys;}

            foreach (var entityUId in minCountEntityUIds)
            {
                T0 t0_comp;if(!t0_dict.TryGetValue(entityUId,out t0_comp)) continue;
                T1 t1_comp;if(!t1_dict.TryGetValue(entityUId,out t1_comp)) continue;
                T2 t2_comp;if(!t2_dict.TryGetValue(entityUId,out t2_comp)) continue;
                T3 t3_comp;if(!t3_dict.TryGetValue(entityUId,out t3_comp)) continue;
                T4 t4_comp;if(!t4_dict.TryGetValue(entityUId,out t4_comp)) continue;
                T5 t5_comp;if(!t5_dict.TryGetValue(entityUId,out t5_comp)) continue;
                T6 t6_comp;if(!t6_dict.TryGetValue(entityUId,out t6_comp)) continue;

                callback(ref t0_comp,ref t1_comp,ref t2_comp,ref t3_comp,ref t4_comp,ref t5_comp,ref t6_comp);

                if(t0_comp.Dirty>0)t0_dict[entityUId]=t0_comp;
                if(t1_comp.Dirty>0)t1_dict[entityUId]=t1_comp;
                if(t2_comp.Dirty>0)t2_dict[entityUId]=t2_comp;
                if(t3_comp.Dirty>0)t3_dict[entityUId]=t3_comp;
                if(t4_comp.Dirty>0)t4_dict[entityUId]=t4_comp;
                if(t5_comp.Dirty>0)t5_dict[entityUId]=t5_comp;
                if(t6_comp.Dirty>0)t6_dict[entityUId]=t6_comp;
            }
        }     


        public void Filter<T0,T1,T2,T3,T4,T5,T6,T7>(WorldFilterDelegate_R8<T0,T1,T2,T3,T4,T5,T6,T7> callback)
        where T0:struct,IComponent
        where T1:struct,IComponent
        where T2:struct,IComponent
        where T3:struct,IComponent
        where T4:struct,IComponent
        where T5:struct,IComponent
        where T6:struct,IComponent
        where T7:struct,IComponent
        {
            var t0_tVal = ComponentTypeHelper.GetTypeValue<T0>();
            var t1_tVal = ComponentTypeHelper.GetTypeValue<T1>();
            var t2_tVal = ComponentTypeHelper.GetTypeValue<T2>();
            var t3_tVal = ComponentTypeHelper.GetTypeValue<T3>();
            var t4_tVal = ComponentTypeHelper.GetTypeValue<T4>();
            var t5_tVal = ComponentTypeHelper.GetTypeValue<T5>();
            var t6_tVal = ComponentTypeHelper.GetTypeValue<T6>();
            var t7_tVal = ComponentTypeHelper.GetTypeValue<T7>();

            if
            (
                !_compDict.ContainsKey(t0_tVal) || _compDict[t0_tVal].Count==0
                || !_compDict.ContainsKey(t1_tVal) || _compDict[t1_tVal].Count==0
                || !_compDict.ContainsKey(t2_tVal) || _compDict[t2_tVal].Count==0
                || !_compDict.ContainsKey(t3_tVal) || _compDict[t3_tVal].Count==0
                || !_compDict.ContainsKey(t4_tVal) || _compDict[t4_tVal].Count==0
                || !_compDict.ContainsKey(t5_tVal) || _compDict[t5_tVal].Count==0
                || !_compDict.ContainsKey(t6_tVal) || _compDict[t6_tVal].Count==0
                || !_compDict.ContainsKey(t7_tVal) || _compDict[t7_tVal].Count==0
            )return;

            Dictionary<int, T0> t0_dict = _compDict[t0_tVal] as Dictionary<int, T0>;
            Dictionary<int, T1> t1_dict = _compDict[t1_tVal] as Dictionary<int, T1>;
            Dictionary<int, T2> t2_dict = _compDict[t2_tVal] as Dictionary<int, T2>;
            Dictionary<int, T3> t3_dict = _compDict[t3_tVal] as Dictionary<int, T3>;
            Dictionary<int, T4> t4_dict = _compDict[t4_tVal] as Dictionary<int, T4>;
            Dictionary<int, T5> t5_dict = _compDict[t5_tVal] as Dictionary<int, T5>;
            Dictionary<int, T6> t6_dict = _compDict[t6_tVal] as Dictionary<int, T6>;
            Dictionary<int, T7> t7_dict = _compDict[t7_tVal] as Dictionary<int, T7>;

            ICollection<int> minCountEntityUIds = null;
            int minCount = int.MaxValue;

            if(t0_dict.Count<minCount){minCount = t0_dict.Count;minCountEntityUIds = t0_dict.Keys;}
            if(t1_dict.Count<minCount){minCount = t1_dict.Count;minCountEntityUIds = t1_dict.Keys;}
            if(t2_dict.Count<minCount){minCount = t2_dict.Count;minCountEntityUIds = t2_dict.Keys;}
            if(t3_dict.Count<minCount){minCount = t3_dict.Count;minCountEntityUIds = t3_dict.Keys;}
            if(t4_dict.Count<minCount){minCount = t4_dict.Count;minCountEntityUIds = t4_dict.Keys;}
            if(t5_dict.Count<minCount){minCount = t5_dict.Count;minCountEntityUIds = t5_dict.Keys;}
            if(t6_dict.Count<minCount){minCount = t6_dict.Count;minCountEntityUIds = t6_dict.Keys;}
            if(t7_dict.Count<minCount){minCount = t7_dict.Count;minCountEntityUIds = t7_dict.Keys;}

            foreach (var entityUId in minCountEntityUIds)
            {
                T0 t0_comp;if(!t0_dict.TryGetValue(entityUId,out t0_comp)) continue;
                T1 t1_comp;if(!t1_dict.TryGetValue(entityUId,out t1_comp)) continue;
                T2 t2_comp;if(!t2_dict.TryGetValue(entityUId,out t2_comp)) continue;
                T3 t3_comp;if(!t3_dict.TryGetValue(entityUId,out t3_comp)) continue;
                T4 t4_comp;if(!t4_dict.TryGetValue(entityUId,out t4_comp)) continue;
                T5 t5_comp;if(!t5_dict.TryGetValue(entityUId,out t5_comp)) continue;
                T6 t6_comp;if(!t6_dict.TryGetValue(entityUId,out t6_comp)) continue;
                T7 t7_comp;if(!t7_dict.TryGetValue(entityUId,out t7_comp)) continue;

                callback(ref t0_comp,ref t1_comp,ref t2_comp,ref t3_comp,ref t4_comp,ref t5_comp,ref t6_comp,ref t7_comp);

                if(t0_comp.Dirty>0)t0_dict[entityUId]=t0_comp;
                if(t1_comp.Dirty>0)t1_dict[entityUId]=t1_comp;
                if(t2_comp.Dirty>0)t2_dict[entityUId]=t2_comp;
                if(t3_comp.Dirty>0)t3_dict[entityUId]=t3_comp;
                if(t4_comp.Dirty>0)t4_dict[entityUId]=t4_comp;
                if(t5_comp.Dirty>0)t5_dict[entityUId]=t5_comp;
                if(t6_comp.Dirty>0)t6_dict[entityUId]=t6_comp;
                if(t7_comp.Dirty>0)t7_dict[entityUId]=t7_comp;
            }
        }     

    }
}
