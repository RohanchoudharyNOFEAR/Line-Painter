using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LinePaint
{
    [CreateAssetMenu(fileName ="LevelData",menuName = "ScriptableObjects/Create LevelData",order =1)]
    public class LevelScriptableData : ScriptableObject
    {
        public int Width, Height;
        public Vector2Int BrushStartCoords;
       public  List<Connection> completePattern = new List<Connection>();
    }
}
