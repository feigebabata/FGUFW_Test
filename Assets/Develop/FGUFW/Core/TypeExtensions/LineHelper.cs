using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace FGUFW
{
    public static class LineHelper
    {
        public static Vector3[] GetParallel(Vector3[] line,Vector3 up,float offset)
        {
            var newLine = new Vector3[line.Length];
            up = up.normalized;
            for (int i = 0; i < line.Length; i++)
            {
                Vector3 point = line[i];
                Vector3 lineDir;
                if(i==0)
                {
                    lineDir = (line[i+1]-line[i]).normalized;
                }
                else
                {
                    lineDir = (line[i]-line[i-1]).normalized;
                }
                var newPointDir = Vector3.Cross(lineDir,up);
                var newPoint = point + newPointDir*offset;
                newLine[i]=newPoint;
            }
            return newLine;
        }

    }

}