using System.Collections;
using System.Collections.Generic;
using FGUFW.ECS;
using UnityEngine;

namespace GUNNAC
{
    public class ColliderDisposableComp : MonoBehaviour,IBulletAttacker
    {
        public int EntityUId{get;set;}

        /// <summary>
        /// OnTriggerEnter is called when the Collider other enters the trigger.
        /// </summary>
        /// <param name="other">The other Collider involved in this collision.</param>
        void OnTriggerEnter(Collider other)
        {
            var caComp = other.GetComponent<IAttacked>();
            if(caComp==null)return;
            var bcMsgComp = new BattleCollisionDisposableMsgComp(EntityUId);
            bcMsgComp.TargetEUId = caComp.EntityUId;
            World.Current.AddOrSetComponent(EntityUId,bcMsgComp);
        }


    }
}
