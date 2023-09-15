using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace FGUFW
{

    public static class MaterialPropertyBlockHelper
    {
        private static Dictionary<int,MaterialPropertyBlock> materialPropertyBlockCache = new Dictionary<int, MaterialPropertyBlock>();

        public static void Clear()
        {
            materialPropertyBlockCache.Clear();
        }
        
        public static void SetMaterialProperty(this Renderer self,int nameID,float value)
        {
            var mpb = getMPB(nameID);
            mpb.SetFloat(nameID,value);
            self.SetPropertyBlock(mpb);
        }
        
        public static void SetMaterialProperty(this Renderer self,int nameID,Color value)
        {
            var mpb = getMPB(nameID);
            mpb.SetColor(nameID,value);
            self.SetPropertyBlock(mpb);
        }
        
        public static void SetMaterialProperty(this Renderer self,int nameID,Vector4 value)
        {
            var mpb = getMPB(nameID);
            mpb.SetVector(nameID,value);
            self.SetPropertyBlock(mpb);
        }

        private static MaterialPropertyBlock getMPB(int nameID)
        {
            if(!materialPropertyBlockCache.ContainsKey(nameID))
            {
                materialPropertyBlockCache.Add(nameID,new MaterialPropertyBlock());
            }
            return materialPropertyBlockCache[nameID];
        }

    }
}