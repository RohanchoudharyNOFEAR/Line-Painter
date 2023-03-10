using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LinePaint
{
    public class Grid 
    {
        private int _width;
        private int _height;
        private float _cellSize;

        private int[,] gridArray;

        public int[,] GridArray { get { return gridArray; } }

        public void Initialize(int width, int height, float cellSize)
        {
            _width = width;
            _height = height;
            _cellSize = cellSize;

            gridArray = new int[width, height];
        }

        public Vector3 GetCellWorldPosition(int x, int y)
        {
            return new Vector3(Mathf.FloorToInt(_cellSize * x), 0, Mathf.FloorToInt(_cellSize * y));
        }
        

    }
}
