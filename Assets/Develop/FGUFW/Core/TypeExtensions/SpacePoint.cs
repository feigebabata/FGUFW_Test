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
        private int[] _gridSizes;
        private Vector3 _center,_girdSize,_spaceMinPoint,_spaceMaxPoint;

        //----------------------------------------------

        private int _newPointIndex=0;
        public int PointCount{get;private set;}

        /// <summary>
        /// 所有点索引的集合 按格子顺序
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
            _gridSizes = new int[GridLength];
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
        /// 遍历某个格子里所有点的索引
        /// </summary>
        /// <param name="gridIndex"></param>
        /// <param name="callback"></param>
        public void ForeachGrid(int gridIndex,Action<int> callback)
        {
            if(callback==null)return;

        }

        /// <summary>
        /// 添加新点对象 返回点索引
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public int Add(Vector3 point)
        {
            int index = _newPointIndex;
            _newPointIndex++;
            PointCount++;
            if(PointCount>_pointIndexs.Length)ensureCapacity();

            return index;
        }

        /// <summary>
        /// 移除点
        /// </summary>
        /// <param name="pointIndex"></param>
        public void Remove(int pointIndex)
        {
            PointCount--;
        }

        /// <summary>
        /// 点位置改变 返回移动后所在格子索引
        /// </summary>
        /// <param name="pointIndex"></param>
        /// <param name="newPos"></param>
        /// <returns></returns>
        public int PointMove(int pointIndex,Vector3 newPos)
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

        

    }
}