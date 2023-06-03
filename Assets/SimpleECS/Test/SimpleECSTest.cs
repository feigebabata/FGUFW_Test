using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace FGUFW.SimpleECS.Test
{
    public class SimpleECSTest : MonoBehaviour
    {
        public Transform monster;

        TestWorld _world;

        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        void Start()
        {
            _world = new TestWorld();
            _world.MonsterGroup = new MonsterComponentGroup(monster.gameObject);
            
            var ids = _world.MonsterGroup.Instantiate(1);

        }

        /// <summary>
        /// This function is called when the MonoBehaviour will be destroyed.
        /// </summary>
        void OnDestroy()
        {
            _world.Dispose();
        }

    }
}
