using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LinePaint
{
    public class LevelManager : MonoBehaviour
    {
        public GameObject block;
        [SerializeField] private float _cellSize;
        [SerializeField] private int _heigth;
        [SerializeField] private int _width;
        private Grid grid;
        private SwipeController swipeControl;

        // Start is called before the first frame update
        void Start()
        {
            grid = new Grid();
            swipeControl = new SwipeController();
            swipeControl.SetLevelManager(this);
            grid.Initialize(_width, _heigth, _cellSize);
            CreateGrid();
        }

        // Update is called once per frame
        void Update()
        {
            if(swipeControl!=null)
            {
                swipeControl.OnUpdate();
            }

        }


        private void CreateGrid()
        {
            for (int x = 0; x < grid.GridArray.GetLength(0); x++)
            {
                for (int y = 0; y < grid.GridArray.GetLength(1); y++)
                {
                    GameObject cell = Instantiate(block);
                    cell.transform.position = grid.GetCellWorldPosition(x, y);
                }
            }
        }

       public void MoveBrush(Swipe direction)
        {
            Debug.Log(direction);

        }



    }
}
