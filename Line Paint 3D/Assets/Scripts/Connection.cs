using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LinePaint
{
    [System.Serializable]
    public class Connection 
    {

       [SerializeField] private Vector2Int _startCoords;
       [SerializeField] private Vector2Int _endCoords;

        public Vector2Int StartCoords { get => _startCoords; }
        public Vector2Int EndCoords { get => _endCoords; }

        public Connection(Vector2Int startCoord, Vector2Int endCoord)
        {
            _startCoords = startCoord;
            _endCoords= endCoord;
        }
    }
}
