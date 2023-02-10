using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGUFW;

namespace FGUFW.FlowFieldPathfinding
{
    public class FlowFielder : MonoBehaviour
    {
        public bool DrawGizmos=true;
        public Bounds Space;
        public Vector2Int GridCount;

        /// <summary>
        /// 平面法线
        /// </summary>
        public bool YorZ=>Space.size.y==0;

        private SpaceV2 _spaceV2;

        // Start is called before the first frame update
        void Start()
        {
            
        }


        /// <summary>
        /// Callback to draw gizmos that are pickable and always drawn.
        /// </summary>
        void OnDrawGizmos()
        {
            
        }

        public GridV2[] CreateFlowField()
        {
            int length = GridCount.x*GridCount.y;
            var grids = new GridV2[length];
            var collider2Ds = GameObject.FindObjectsOfType<Obstacle>().To<Obstacle,Collider2D>(obst=>obst.GetComponent<Collider2D>());
            for (int i = 0; i < length; i++)
            {
                
            }
            return grids;
        }

        private bool inObstacle(Vector2 point,Collider2D[] collider2Ds)
        {
            foreach (var collider2D in collider2Ds)
            {
                if(collider2D.OverlapPoint(point))
                {
                    return true;
                }
            }
            return false;
        }

        public Vector3 Index2Point(int index)
        {
            var point = Vector3.zero;
            var gridIndex = SpaceV2.Index2Position(index,GridCount.x);
            var gridSize = GridSize();
            Vector3 start = gridSize*0.5f;
            start+=Space.min;
            point.x = start.x + gridSize.x*gridIndex.x;
            if(YorZ)
            {
                point.z = start.y + gridSize.y*gridIndex.y;
            }
            else
            {
                point.y = start.y + gridSize.y*gridIndex.y;
            }
            return point;
        }

        public Vector2Int Point2GridIndex(Vector3 point)
        {
            Vector2Int gridIndex = Vector2Int.one*-1;
            if(!Space.Contains(point))return gridIndex;
            var offset = point-Space.min;
            var gridSize = GridSize();
            gridIndex.x = (int)(offset.x/gridSize.x);
            if(YorZ)
            {
                gridIndex.y = (int)(offset.z/gridSize.y);
            }
            else
            {
                gridIndex.y = (int)(offset.z/gridSize.y);
            }
            return gridIndex;
        }
        
        public Vector2 GridSize()
        {
            if(YorZ)
            {
                return new Vector2(Space.size.x/GridCount.x,Space.size.z/GridCount.y);
            }
            else
            {
                return new Vector2(Space.size.x/GridCount.x,Space.size.y/GridCount.y);
            }
        }



    }

}
