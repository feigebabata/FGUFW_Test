using System;
using UnityEngine;

namespace FGUFW
{
    /// <summary>
    /// 空间划分 点模式
    /// 空间由X*Y*Z个格子构成 格子顺序X >> Y >> Z
    /// 点在格子内: gridMin >= 点 < gridMax
    /// 空间之外点的所在格子索引为-1
    /// </summary>
    public sealed class SpacePoint
    {
        private const int EXPAND = 1024;

        public int GridLength{get;private set;}
        public int X{get;private set;}
        public int Y{get;private set;}
        public int Z{get;private set;}

        /// <summary>
        /// 每个格子里点的数量
        /// </summary>
        private int[] _gridCapacitys;
        private Vector3 _center,_girdSize,_spaceMinPoint,_spaceMaxPoint;

        //----------------------------------------------

        private int _newPointId=0;
        public int PointCount{get;private set;}

        /// <summary>
        /// 所有点索引的集合 按格子顺序 先存outSide
        /// </summary>
        private int[] _pointIndexs;
        private Vector3[] _points;

        private SpacePoint(){}

        public SpacePoint(Vector3Int spaceSize, Vector3 gridSize,Vector3 center,int capacity=8)
        {
            GridLength = spaceSize.x*spaceSize.y*spaceSize.z;
            X = spaceSize.x;
            Y = spaceSize.y;
            Z = spaceSize.z;
            _gridCapacitys = new int[GridLength+1];
            _center = center;
            _girdSize = gridSize;

            Vector3 half = new Vector3(gridSize.x*X,gridSize.y*Y,gridSize.z*Z)*0.5f;
            _spaceMinPoint = center - half;
            _spaceMaxPoint = center + half;

            if(capacity<=0)capacity=8;
            _pointIndexs = new int[capacity];
            _points = new Vector3[capacity];
        }

        /// <summary>
        /// 遍历某个格子里所有点的Id
        /// </summary>
        public void ForeachGrid(int gridIndex,Action<int> callback)
        {
            if(callback==null)return;

        }

        /// <summary>
        /// 添加新点对象 返回点Id
        /// </summary>
        public int Add(Vector3 point)
        {
            int newIndex,oldIndex;
            Vector3 oldPoint,newPoint;

            int gridIndex = FindGridIndex(point);

            newIndex = _newPointId;
            newPoint = point;
            _newPointId++;

            if(PointCount+1>_pointIndexs.Length)ensureCapacity();
            
            int pointIndex = 0;
            for (int i = -1; i < GridLength; i++)
            {
                int capacity = _gridCapacitys[i+1];
                pointIndex+=capacity;
                if(i==gridIndex)
                {
                    newIndex = pointIndex;
                    oldIndex = _pointIndexs[pointIndex];
                    oldPoint = _points[pointIndex];

                    _pointIndexs[pointIndex] = newIndex;
                    _points[pointIndex] = newPoint;
                }
                else if(i>gridIndex && capacity>0)
                {
                    oldIndex = _pointIndexs[pointIndex];
                    oldPoint = _points[pointIndex];

                    _pointIndexs[pointIndex] = newIndex;
                    _points[pointIndex] = newPoint;
                }
            }
            PointCount++;
            _gridCapacitys[gridIndex+1]++;

            return _newPointId-1;
        }

        /// <summary>
        /// 移除点
        /// </summary>
        public void Remove(int pointId)
        {
            int newIndex,oldIndex;
            Vector3 oldPoint,newPoint;

            // newIndex = _pointIndexs.IndexOf(pointId);
            // int gridIndex = FindGridIndex(_points[newIndex]);

            // int index = 0;
            // for (int i = -1; i < GridLength; i++)
            // {
            //     int capacity = _gridCapacitys[i+1];
            //     index+=capacity;
            //     if(i==gridIndex)
            //     {
            //         oldIndex = _pointIndexs[index];
            //         oldPoint = _points[index];

            //         _pointIndexs[index] = newIndex;
            //         _points[index] = newPoint;
            //     }
            //     else if(i>gridIndex && capacity>0)
            //     {
            //         oldIndex = _pointIndexs[index];
            //         oldPoint = _points[index];

            //         _pointIndexs[index] = newIndex;
            //         _points[index] = newPoint;
            //     }
            // }

            PointCount--;
        }

        /// <summary>
        /// 点位置改变 返回移动后所在格子Id
        /// </summary>
        public int PointMove(int pointId,Vector3 newPos)
        {
            int gridIndex = -1;

            return gridIndex;
        }

        private void ensureCapacity() 
        {
            int capacity = _pointIndexs.Length;
            var expand = Math.Min(capacity,EXPAND);
            var newCapacity = capacity+expand;
            int[] newPointIndexs = new int[newCapacity];
            Vector3[] newPoints = new Vector3[newCapacity];
            Array.Copy(_pointIndexs,newPointIndexs,capacity);
            Array.Copy(_points,newPoints,capacity);
            _pointIndexs = newPointIndexs;
            _points = newPoints;
        }

        /// <summary>
        /// 获取格子容量
        /// </summary>
        public int GetGridCapacity(int gridIndex)
        {
            return _gridCapacitys[gridIndex+1];
        }

        /// <summary>
        /// 获取点在空间中的格子索引
        /// </summary>
        public int FindGridIndex(Vector3 point)
        {
            int gridIndex = -1;
            int idx_x = MathHelper.IndexOf(X,point.x,_spaceMaxPoint.x-_spaceMinPoint.x,_spaceMinPoint.x);
            int idx_y = MathHelper.IndexOf(Y,point.y,_spaceMaxPoint.y-_spaceMinPoint.y,_spaceMinPoint.y);
            int idx_z = MathHelper.IndexOf(Z,point.z,_spaceMaxPoint.z-_spaceMinPoint.z,_spaceMinPoint.z);
            if(idx_x==-1 || idx_y==-1 || idx_z==-1)
            {
                return gridIndex;
            }
            gridIndex = idx_z*X*Y + idx_y*X + idx_x;
            return gridIndex;
        }

        

    }
}