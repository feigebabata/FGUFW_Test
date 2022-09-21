using System.Collections;
using System.Collections.Generic;
using FGUFW.ECS;
using Unity.Mathematics;
using UnityEngine;

namespace ECSTest
{
    public class ECSTestComp : MonoBehaviour
    {
        public Vector3 RotateTo;
        public float RotateSpeed = 45;

        /// <summary>
        /// Update is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        void LateUpdate()
        {
            var dir = transform.forward;
            var angle = math.acos(math.dot( RotateTo.normalized, dir)) / math.PI * 180;
            if(angle<0.5f)
            {
                transform.up = RotateTo;
                return;
            }
            var rotateDelta = math.min(RotateSpeed * Time.fixedDeltaTime,angle);

            float4 axis = new float4(math.cross(RotateTo, dir),1);
            var matrix = float4x4.AxisAngle(axis.xyz, math.radians(rotateDelta));

            var newDir = math.mul(new float4(dir,1), matrix);

            transform.up = newDir.xyz;
        }


    }

}
