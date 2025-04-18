using System.Collections;
using System.Collections.Generic;
using FGUFW.MonoGameplay;
using UnityEngine;
using FGUFW;
using static FGUsing;

namespace Test
{
    public class TestPlay : Play<TestPlay>
    {
        public override IEnumerator OnCreating(Part play,Part parent)
        {
            //AddPart<MonoGameplayTestPart>();
            
            yield return base.OnCreating(this,this);
        }

        protected override void OnDispose()
        {
            base.OnDispose();
        }
    }
}

