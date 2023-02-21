using System;
using System.Collections.Generic;
using FGUFW;
using UnityEngine;

namespace FGUFW.FlowFieldPathfinding
{
    public unsafe sealed class Space
    {
        private readonly Grid[] _grids;
        public readonly Vector3Int GridCount;
        public readonly Vector3 Min,Max;

        private Space(){}

        public Space(Vector3Int girdCount,Vector3 space_min,Vector3 space_max,Grid[] grids=null)
        {
            GridCount = girdCount;
            Min = space_min;
            Max = space_max;

            int length = girdCount.x*girdCount.y*girdCount.z;
            if(grids==null || grids.Length!=length)
            {
                _grids = new Grid[length];
            }
            else
            {
                _grids = grids;
            }
        }

        public Grid this[int index]
        {
            get
            {
                if(index>=0 && index<_grids.Length)
                {
                    return _grids[index];
                }
                return default(Grid);
            }
            set
            {
                if(value && index>=0 && index<_grids.Length)
                {
                    _grids[index]=value;
                }
            }
        }

        public Grid this[Vector3Int coord]
        {
            get
            {
                int index = GeometryHelper.SpaceCoord2Index(GridCount,coord);
                return this[index];
            }
            set
            {
                int index = GeometryHelper.SpaceCoord2Index(GridCount,coord);
                this[index] = value;
            }
        }

        public Grid this[Vector3 point]
        {
            get
            {
                int index = GeometryHelper.SpacePoint2Index(Min,Max,point,GridCount);
                return this[index];
            }
            set
            {
                int index = GeometryHelper.SpacePoint2Index(Min,Max,point,GridCount);
                this[index] = value;
            }
        }

        public Space Clone()
        {
            var grids = new Grid[_grids.Length];
            Array.Copy(_grids,grids,_grids.Length);
            return new Space(GridCount,Min,Max,grids);
        }

        public bool Flow(Vector3 point,int landforms)
        {
            var start = GeometryHelper.SpacePoint2Coord(Min,Max,point,GridCount);
            return Flow(start,landforms);
        }

        /// <summary>
        /// 根据是否允许经过地貌而生成流场
        /// </summary>
        /// <param name="end"></param>
        /// <param name="landforms"></param>
        public bool Flow(Vector3Int end,int landforms)
        {
            var grid_idx = GeometryHelper.SpaceCoord2Index(GridCount,end);
            if(grid_idx==-1)return false;

            resetGrids();

            var queueCapacity = _grids.Length;
            var queueArray = stackalloc Vector3Int[queueCapacity];
            var queue = new StackMemory_Queue<Vector3Int>(queueArray,queueCapacity);
            
            var girdNearCoords = stackalloc Vector3Int[6]; 
            var gridComplete = stackalloc bool[queueCapacity];

            _grids[grid_idx].Distance = 0;
            queue.Enqueue(end);

            //广度优先遍历所有格子
            while (queue.Count>0)
            {
                //处理当前格子附近6个格子
                var center_coord = queue.Dequeue();
                grid_idx = GeometryHelper.SpaceCoord2Index(GridCount,center_coord);
                gridComplete[grid_idx] = true;

                girdNearCoords[0] = center_coord+Vector3Int.forward;
                girdNearCoords[1] = center_coord+Vector3Int.back;
                girdNearCoords[2] = center_coord+Vector3Int.left;
                girdNearCoords[3] = center_coord+Vector3Int.right;
                girdNearCoords[4] = center_coord+Vector3Int.up;
                girdNearCoords[5] = center_coord+Vector3Int.down;


                var center = _grids[grid_idx];

                for (int i = 0; i < 6; i++)
                {
                    var grid_coord = girdNearCoords[i];
                    grid_idx = GeometryHelper.SpaceCoord2Index(GridCount,grid_coord);
                    if(grid_idx!=-1 && BitHelper.Contains(landforms,_grids[grid_idx].Landform))
                    {
                        float distance = center.Distance + _grids[grid_idx].Weight;
                        //格子未被赋值或新距离更小
                        if(_grids[grid_idx].Distance==Grid.NoneDistance || _grids[grid_idx].Distance>distance)
                        {
                            _grids[grid_idx].Distance = distance;
                        }

                        //格子未被处理完且队列未包含
                        if(!gridComplete[grid_idx] && !queue.Contains(grid_coord))
                        {
                            queue.Enqueue(grid_coord);
                        }
                    }
                }
            }
            return true;
        }

        public void MoveTo(Vector3 point,Action<Vector3> callback)
        {

        }

        public void MoveTo(Vector3 point,Action<Vector3Int> callback)
        {
            
        }

        public void MoveTo(Vector3Int start,Action<Vector3> callback)
        {

        }

        public void MoveTo(Vector3Int start,Action<Vector3Int> callback)
        {
            
        }



        /// <summary>
        /// 重制所有格子
        /// </summary>
        private void resetGrids()
        {
            for (int i = 0; i < _grids.Length; i++)
            {
                _grids[i].Distance = Grid.NoneDistance;
            }
        }

    }

    public struct Grid
    {
        public Grid(int landform,float weight)
        {
            Landform = landform;
            Weight = weight;
            Distance = NoneDistance;
        }

        /// <summary>
        /// 地形 2的指数幂 0为无效值
        /// </summary>
        public readonly int Landform;

        /// <summary>
        /// 距离权重
        /// </summary>
        public readonly float Weight;

        /// <summary>
        /// 距离
        /// </summary>
        public float Distance;

        /// <summary>
        /// 无距离
        /// </summary>
        public const float NoneDistance = -1;

        public static implicit operator bool(Grid exists)
        {
            return exists.Landform!=0;
        }
    }
}