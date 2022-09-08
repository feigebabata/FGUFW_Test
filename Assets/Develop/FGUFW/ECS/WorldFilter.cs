
using System;
using FGUFW;

namespace FGUFW.ECS
{
    public partial class World
    {
        

        public void Filter<T0>(Action<T0> callback)
        where T0:Component
        {
            var t0_tVal = ComponentTypeHelper.GetTypeValue<T0>();

            if
            (
                !_compDict.ContainsKey(t0_tVal) || _compDict[t0_tVal].Count==0
            )return;

            int minCountType = -1;
            int minCount = int.MaxValue;

            if(_compDict[t0_tVal].Count<minCount)
            {
                minCount = _compDict[t0_tVal].Count;
                minCountType = t0_tVal;
            }

            var minComps = _compDict[minCountType];
            
            foreach (var item in minComps)
            {
                var entityUId = item.EntityUId;

                var t0_comp = GetComponent<T0>(entityUId);if(t0_comp==null)continue;

                callback(t0_comp);

            }
        }     


        public void Filter<T0,T1>(Action<T0,T1> callback)
        where T0:Component
        where T1:Component
        {
            var t0_tVal = ComponentTypeHelper.GetTypeValue<T0>();
            var t1_tVal = ComponentTypeHelper.GetTypeValue<T1>();

            if
            (
                !_compDict.ContainsKey(t0_tVal) || _compDict[t0_tVal].Count==0
                || !_compDict.ContainsKey(t1_tVal) || _compDict[t1_tVal].Count==0
            )return;

            int minCountType = -1;
            int minCount = int.MaxValue;

            if(_compDict[t0_tVal].Count<minCount)
            {
                minCount = _compDict[t0_tVal].Count;
                minCountType = t0_tVal;
            }
            if(_compDict[t1_tVal].Count<minCount)
            {
                minCount = _compDict[t1_tVal].Count;
                minCountType = t1_tVal;
            }

            var minComps = _compDict[minCountType];
            
            foreach (var item in minComps)
            {
                var entityUId = item.EntityUId;

                var t0_comp = GetComponent<T0>(entityUId);if(t0_comp==null)continue;
                var t1_comp = GetComponent<T1>(entityUId);if(t1_comp==null)continue;

                callback(t0_comp,t1_comp);

            }
        }     


        public void Filter<T0,T1,T2>(Action<T0,T1,T2> callback)
        where T0:Component
        where T1:Component
        where T2:Component
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

            int minCountType = -1;
            int minCount = int.MaxValue;

            if(_compDict[t0_tVal].Count<minCount)
            {
                minCount = _compDict[t0_tVal].Count;
                minCountType = t0_tVal;
            }
            if(_compDict[t1_tVal].Count<minCount)
            {
                minCount = _compDict[t1_tVal].Count;
                minCountType = t1_tVal;
            }
            if(_compDict[t2_tVal].Count<minCount)
            {
                minCount = _compDict[t2_tVal].Count;
                minCountType = t2_tVal;
            }

            var minComps = _compDict[minCountType];
            
            foreach (var item in minComps)
            {
                var entityUId = item.EntityUId;

                var t0_comp = GetComponent<T0>(entityUId);if(t0_comp==null)continue;
                var t1_comp = GetComponent<T1>(entityUId);if(t1_comp==null)continue;
                var t2_comp = GetComponent<T2>(entityUId);if(t2_comp==null)continue;

                callback(t0_comp,t1_comp,t2_comp);

            }
        }     


        public void Filter<T0,T1,T2,T3>(Action<T0,T1,T2,T3> callback)
        where T0:Component
        where T1:Component
        where T2:Component
        where T3:Component
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

            int minCountType = -1;
            int minCount = int.MaxValue;

            if(_compDict[t0_tVal].Count<minCount)
            {
                minCount = _compDict[t0_tVal].Count;
                minCountType = t0_tVal;
            }
            if(_compDict[t1_tVal].Count<minCount)
            {
                minCount = _compDict[t1_tVal].Count;
                minCountType = t1_tVal;
            }
            if(_compDict[t2_tVal].Count<minCount)
            {
                minCount = _compDict[t2_tVal].Count;
                minCountType = t2_tVal;
            }
            if(_compDict[t3_tVal].Count<minCount)
            {
                minCount = _compDict[t3_tVal].Count;
                minCountType = t3_tVal;
            }

            var minComps = _compDict[minCountType];
            
            foreach (var item in minComps)
            {
                var entityUId = item.EntityUId;

                var t0_comp = GetComponent<T0>(entityUId);if(t0_comp==null)continue;
                var t1_comp = GetComponent<T1>(entityUId);if(t1_comp==null)continue;
                var t2_comp = GetComponent<T2>(entityUId);if(t2_comp==null)continue;
                var t3_comp = GetComponent<T3>(entityUId);if(t3_comp==null)continue;

                callback(t0_comp,t1_comp,t2_comp,t3_comp);

            }
        }     


        public void Filter<T0,T1,T2,T3,T4>(Action<T0,T1,T2,T3,T4> callback)
        where T0:Component
        where T1:Component
        where T2:Component
        where T3:Component
        where T4:Component
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

            int minCountType = -1;
            int minCount = int.MaxValue;

            if(_compDict[t0_tVal].Count<minCount)
            {
                minCount = _compDict[t0_tVal].Count;
                minCountType = t0_tVal;
            }
            if(_compDict[t1_tVal].Count<minCount)
            {
                minCount = _compDict[t1_tVal].Count;
                minCountType = t1_tVal;
            }
            if(_compDict[t2_tVal].Count<minCount)
            {
                minCount = _compDict[t2_tVal].Count;
                minCountType = t2_tVal;
            }
            if(_compDict[t3_tVal].Count<minCount)
            {
                minCount = _compDict[t3_tVal].Count;
                minCountType = t3_tVal;
            }
            if(_compDict[t4_tVal].Count<minCount)
            {
                minCount = _compDict[t4_tVal].Count;
                minCountType = t4_tVal;
            }

            var minComps = _compDict[minCountType];
            
            foreach (var item in minComps)
            {
                var entityUId = item.EntityUId;

                var t0_comp = GetComponent<T0>(entityUId);if(t0_comp==null)continue;
                var t1_comp = GetComponent<T1>(entityUId);if(t1_comp==null)continue;
                var t2_comp = GetComponent<T2>(entityUId);if(t2_comp==null)continue;
                var t3_comp = GetComponent<T3>(entityUId);if(t3_comp==null)continue;
                var t4_comp = GetComponent<T4>(entityUId);if(t4_comp==null)continue;

                callback(t0_comp,t1_comp,t2_comp,t3_comp,t4_comp);

            }
        }     


        public void Filter<T0,T1,T2,T3,T4,T5>(Action<T0,T1,T2,T3,T4,T5> callback)
        where T0:Component
        where T1:Component
        where T2:Component
        where T3:Component
        where T4:Component
        where T5:Component
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

            int minCountType = -1;
            int minCount = int.MaxValue;

            if(_compDict[t0_tVal].Count<minCount)
            {
                minCount = _compDict[t0_tVal].Count;
                minCountType = t0_tVal;
            }
            if(_compDict[t1_tVal].Count<minCount)
            {
                minCount = _compDict[t1_tVal].Count;
                minCountType = t1_tVal;
            }
            if(_compDict[t2_tVal].Count<minCount)
            {
                minCount = _compDict[t2_tVal].Count;
                minCountType = t2_tVal;
            }
            if(_compDict[t3_tVal].Count<minCount)
            {
                minCount = _compDict[t3_tVal].Count;
                minCountType = t3_tVal;
            }
            if(_compDict[t4_tVal].Count<minCount)
            {
                minCount = _compDict[t4_tVal].Count;
                minCountType = t4_tVal;
            }
            if(_compDict[t5_tVal].Count<minCount)
            {
                minCount = _compDict[t5_tVal].Count;
                minCountType = t5_tVal;
            }

            var minComps = _compDict[minCountType];
            
            foreach (var item in minComps)
            {
                var entityUId = item.EntityUId;

                var t0_comp = GetComponent<T0>(entityUId);if(t0_comp==null)continue;
                var t1_comp = GetComponent<T1>(entityUId);if(t1_comp==null)continue;
                var t2_comp = GetComponent<T2>(entityUId);if(t2_comp==null)continue;
                var t3_comp = GetComponent<T3>(entityUId);if(t3_comp==null)continue;
                var t4_comp = GetComponent<T4>(entityUId);if(t4_comp==null)continue;
                var t5_comp = GetComponent<T5>(entityUId);if(t5_comp==null)continue;

                callback(t0_comp,t1_comp,t2_comp,t3_comp,t4_comp,t5_comp);

            }
        }     


        public void Filter<T0,T1,T2,T3,T4,T5,T6>(Action<T0,T1,T2,T3,T4,T5,T6> callback)
        where T0:Component
        where T1:Component
        where T2:Component
        where T3:Component
        where T4:Component
        where T5:Component
        where T6:Component
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

            int minCountType = -1;
            int minCount = int.MaxValue;

            if(_compDict[t0_tVal].Count<minCount)
            {
                minCount = _compDict[t0_tVal].Count;
                minCountType = t0_tVal;
            }
            if(_compDict[t1_tVal].Count<minCount)
            {
                minCount = _compDict[t1_tVal].Count;
                minCountType = t1_tVal;
            }
            if(_compDict[t2_tVal].Count<minCount)
            {
                minCount = _compDict[t2_tVal].Count;
                minCountType = t2_tVal;
            }
            if(_compDict[t3_tVal].Count<minCount)
            {
                minCount = _compDict[t3_tVal].Count;
                minCountType = t3_tVal;
            }
            if(_compDict[t4_tVal].Count<minCount)
            {
                minCount = _compDict[t4_tVal].Count;
                minCountType = t4_tVal;
            }
            if(_compDict[t5_tVal].Count<minCount)
            {
                minCount = _compDict[t5_tVal].Count;
                minCountType = t5_tVal;
            }
            if(_compDict[t6_tVal].Count<minCount)
            {
                minCount = _compDict[t6_tVal].Count;
                minCountType = t6_tVal;
            }

            var minComps = _compDict[minCountType];
            
            foreach (var item in minComps)
            {
                var entityUId = item.EntityUId;

                var t0_comp = GetComponent<T0>(entityUId);if(t0_comp==null)continue;
                var t1_comp = GetComponent<T1>(entityUId);if(t1_comp==null)continue;
                var t2_comp = GetComponent<T2>(entityUId);if(t2_comp==null)continue;
                var t3_comp = GetComponent<T3>(entityUId);if(t3_comp==null)continue;
                var t4_comp = GetComponent<T4>(entityUId);if(t4_comp==null)continue;
                var t5_comp = GetComponent<T5>(entityUId);if(t5_comp==null)continue;
                var t6_comp = GetComponent<T6>(entityUId);if(t6_comp==null)continue;

                callback(t0_comp,t1_comp,t2_comp,t3_comp,t4_comp,t5_comp,t6_comp);

            }
        }     


        public void Filter<T0,T1,T2,T3,T4,T5,T6,T7>(Action<T0,T1,T2,T3,T4,T5,T6,T7> callback)
        where T0:Component
        where T1:Component
        where T2:Component
        where T3:Component
        where T4:Component
        where T5:Component
        where T6:Component
        where T7:Component
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

            int minCountType = -1;
            int minCount = int.MaxValue;

            if(_compDict[t0_tVal].Count<minCount)
            {
                minCount = _compDict[t0_tVal].Count;
                minCountType = t0_tVal;
            }
            if(_compDict[t1_tVal].Count<minCount)
            {
                minCount = _compDict[t1_tVal].Count;
                minCountType = t1_tVal;
            }
            if(_compDict[t2_tVal].Count<minCount)
            {
                minCount = _compDict[t2_tVal].Count;
                minCountType = t2_tVal;
            }
            if(_compDict[t3_tVal].Count<minCount)
            {
                minCount = _compDict[t3_tVal].Count;
                minCountType = t3_tVal;
            }
            if(_compDict[t4_tVal].Count<minCount)
            {
                minCount = _compDict[t4_tVal].Count;
                minCountType = t4_tVal;
            }
            if(_compDict[t5_tVal].Count<minCount)
            {
                minCount = _compDict[t5_tVal].Count;
                minCountType = t5_tVal;
            }
            if(_compDict[t6_tVal].Count<minCount)
            {
                minCount = _compDict[t6_tVal].Count;
                minCountType = t6_tVal;
            }
            if(_compDict[t7_tVal].Count<minCount)
            {
                minCount = _compDict[t7_tVal].Count;
                minCountType = t7_tVal;
            }

            var minComps = _compDict[minCountType];
            
            foreach (var item in minComps)
            {
                var entityUId = item.EntityUId;

                var t0_comp = GetComponent<T0>(entityUId);if(t0_comp==null)continue;
                var t1_comp = GetComponent<T1>(entityUId);if(t1_comp==null)continue;
                var t2_comp = GetComponent<T2>(entityUId);if(t2_comp==null)continue;
                var t3_comp = GetComponent<T3>(entityUId);if(t3_comp==null)continue;
                var t4_comp = GetComponent<T4>(entityUId);if(t4_comp==null)continue;
                var t5_comp = GetComponent<T5>(entityUId);if(t5_comp==null)continue;
                var t6_comp = GetComponent<T6>(entityUId);if(t6_comp==null)continue;
                var t7_comp = GetComponent<T7>(entityUId);if(t7_comp==null)continue;

                callback(t0_comp,t1_comp,t2_comp,t3_comp,t4_comp,t5_comp,t6_comp,t7_comp);

            }
        }     

    }
}
