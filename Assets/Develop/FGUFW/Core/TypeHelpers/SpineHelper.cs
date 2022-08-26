#if SPINE
using Spine;
using UnityEngine;

namespace FGUFW
{
    static public class SpineHelper
    {
        static public Vector3 GetBonePoint(this Skeleton skeleton, string name, ref bool have)
        {
            var pos = Vector3.zero;
            have = getBonePoint(skeleton.RootBone,Vector3.zero,name,ref pos);
            return pos*100;
        }
        
        static public Vector3 GetBonePoint(this Skeleton skeleton, string name)
        {
            var pos = Vector3.zero;
            getBonePoint(skeleton.RootBone, Vector3.zero, name, ref pos);
            return pos * 100;
        }

        static private bool getBonePoint(Bone bone,Vector3 parentPos,string name,ref Vector3 pos)
        {
            var bonePos = parentPos + new Vector3(bone.X,bone.Y,0);
            if(bone.ToString()==name)
            {
                pos = bonePos;
                return true;
            }
            else
            {
                parentPos = bonePos;
                foreach (var item in bone.Children)
                {
                    if(getBonePoint(item,parentPos,name,ref pos))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
#endif