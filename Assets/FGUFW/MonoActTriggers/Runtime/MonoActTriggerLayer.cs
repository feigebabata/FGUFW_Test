
using System;
using System.Collections.Generic;
using UnityEngine;

namespace FGUFW.MonoActTriggers
{
    [Serializable]
    public class MonoActTriggerLayer
    {
        [Serializable]
        public struct Item
        {
            public int Layer;
            public int Level;
        }

        // [SerializeField]
        public List<Item> Items = new List<Item>();

        public bool Set(int layer,int level)
        {
            bool haveLayer = false;
            int length = Items.Count;
            for (int i = 0; i < length; i++)
            {
                var item = Items[i];
                if(item.Layer==layer)
                {
                    haveLayer=true;

                    if(item.Level<level)
                    {
                        item.Level = level;
                        Items[i] = item;
                        return true;
                    }

                    if(item.Level==level)
                    {
                        return true;
                    }
                }
            }

            if(!haveLayer)
            {
                Items.Add(new Item
                {
                    Layer = layer,
                    Level = level,
                });
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Remove(int layer,int level)
        {
            Items.RemoveSwapBack(item=>item.Layer==layer && item.Level==level);
        }

        public bool CanSet(int layer,int level)
        {
            foreach (var item in Items)
            {
                if(item.Layer==layer)
                {
                    if(item.Level==level)
                    {
                        return true;
                    }

                    if(item.Level<level)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public int GetLevel(int layer)
        {
            foreach (var item in Items)
            {
                if(item.Layer==layer)
                {
                    return item.Level;
                }
            }
            return 0;
        }

        public void Clear()
        {
            Items.Clean();
        }

    }
}