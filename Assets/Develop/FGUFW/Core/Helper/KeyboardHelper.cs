using UnityEngine;
using System;
using System.Collections.Generic;

namespace FGUFW
{
    /// <summary>
    /// UInt64为4个key(UInt16)组合
    /// </summary>
    public static class KeyboardHelper
    {
        static IList<KeyCode> listenerKeybord = new List<KeyCode>();

        static IOrderedMessenger<UInt64> keyDownMessenger=new OrderedMessenger<UInt64>();

        public static void AddListener_KeyDown(UInt64 keys,Action callback,int weight=0)
        {
            keyDownMessenger.Add(keys,callback,weight);
        }

        public static void RemoveListener_KeyDown(UInt64 keys,Action callback)
        {
            keyDownMessenger.Remove(keys,callback);
        }

        public static void Abort_KeyDown(UInt64 keys)
        {
            keyDownMessenger.Abort(keys);
        }

        public static UInt64 ToKey(params KeyCode[] keys)
        {
            UInt64 key = 0;
            if(keys==null || keys.Length==0)return key;

            Array.Sort<KeyCode>(keys,(l,r)=>{return l-r;});

            for (int i = 0; i < keys.Length; i++)
            {
                UInt64 val = (UInt64)keys[i];
                val = val<<(i*16);
                key = key & val;
            }

            return key;
        }

        static void checkKeyDownEvent()
        {
            var (keys,count) = getDownKeys();
            var origin = keys;
            for (int i = 0; i < listenerKeybord.Count; i++)
            {
                var key = listenerKeybord[i];
                if(Input.GetKeyDown(key) && !KeyInKeys(key,keys))
                {
                    AddKey(key,ref keys,ref count);
                }
                if(count>=4)break;
            }

            if(keys==origin)return;
        }

        static ValueTuple<UInt64,int> getDownKeys()
        {
            UInt64 keys = 0b_0000000000000000_0000000000000000_0000000000000000_0000000000000000;
            int count = 0;
            int length = listenerKeybord.Count;
            for (int i = 0; i < length; i++)
            {
                var key = listenerKeybord[i];
                if(Input.GetKey(key))
                {
                    AddKey(key,ref keys,ref count);
                }
                if(count>=4)break;
            }
            return (keys,count);
        }

        public static bool KeyInKeys(KeyCode key,UInt64 keys)
        {
            for (int i = 0; i < 4; i++)
            {
                UInt64 val = (UInt64)key;
                val = val<<(i*16);
                UInt64 origin = val;
                if((val & keys)==origin)return true;
            }
            return false;
        }

        public static void AddKey(KeyCode key,ref UInt64 keys,ref int count)
        {
            UInt64 val = (UInt64)key;
            val = val << (count*16);
            keys = keys & val;
            count++;
        }


    }
}