using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGUFW;
using FGUFW.SimpleECS;

namespace FGUFW.SimpleECS.Test
{
    public class TestWorld : World<TestWorld>
    {
        public MonsterComponentGroup MonsterGroup;

        public TestWorld():base()
        {
            
        }

        protected override ISystem<TestWorld>[] getSystem()
        {
            return new ISystem<TestWorld>[]
            {
                new MovementVelocitySystem(),
                new MonsterFlipbookAnimationSystem(),
            };
        }

        protected override void OnDispose()
        {
            MonsterGroup.Dispose();
        }
    }
    
}
