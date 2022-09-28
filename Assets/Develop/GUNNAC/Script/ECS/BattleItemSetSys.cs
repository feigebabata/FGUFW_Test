
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGUFW.ECS;
using Unity.Mathematics;
using Unity.Collections;
using UnityEngine.Jobs;
using Unity.Jobs;
using FGUFW;

namespace GUNNAC
{
    public class BattleItemSetSys : ISystem
    {
        public int Order => 0;

        private World _world;

        public void OnInit(World world)
        {
            _world = world;
        }

        public void OnUpdate()
        {
            
            _world.Filter((ref BattleDataComp battleDataComp,ref PositionComp positionComp,ref RenderComp renderComp)=>
            {
                BattleConfigComp battleConfigComp;
                _world.GetComponent(World.ENTITY_SINGLE,out battleConfigComp);
                const float size = 90;
                float offset = -positionComp.Pos.z;
                int currentIndex = (int)((offset)/size);
                // Debug.Log(currentIndex);
                for (int i = 1; i < 3; i++)
                {
                    int prevIndex = currentIndex-i;
                    if(prevIndex*size+size*2<offset)
                    {
                        if(battleDataComp.Childs.ContainsKey(prevIndex))
                        {
                            GameObject.Destroy(battleDataComp.Childs[prevIndex]);
                            battleDataComp.Childs.Remove(prevIndex);
                            // Debug.Log($"移除:{prevIndex}");
                        }
                    }
                    
                }

                if(!battleDataComp.Childs.ContainsKey(currentIndex))
                {
                    int itemIndex = battleConfigComp.BattleItemIndex[currentIndex];
                    var item = battleConfigComp.BattleItems[itemIndex].Copy(renderComp.GObj.transform);
                    item.transform.localPosition = new Vector3(0,0,currentIndex*size+size);
                    battleDataComp.Childs.Add(currentIndex,item);
                    // Debug.Log($"添加:{currentIndex}");
                }

                
                
            });


        }

        public void Dispose()
        {
            _world = null;
        }

        

    }
}
