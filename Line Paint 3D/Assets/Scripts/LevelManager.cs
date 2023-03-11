using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LinePaint
{
    public class LevelManager : MonoBehaviour
    {
        public Cell block;
        [SerializeField] private float _cellSize;
        [SerializeField] private int _heigth;
        [SerializeField] private int _width;
        [SerializeField] private CameraZoom _gameCamera;
        [SerializeField] private BrushController _brush;
        [SerializeField] private LinePaintScript linePaintPrefab;
        private Grid grid;
        private SwipeController swipeControl;
        private BrushController _currentBrush;
        private List<Connection> inProgressPattern = new List<Connection>();
        private List<LinePaintScript> connectedLinePaints = new List<LinePaintScript>();
        private Cell[,] cells;
        

        // Start is called before the first frame update
        void Start()
        {
            grid = new Grid();
            swipeControl = new SwipeController();
            swipeControl.SetLevelManager(this);
            grid.Initialize(_width, _heigth, _cellSize,Vector3.zero);
           
            _gameCamera.ZoomPerspectiveCamera(_width, _heigth);
            cells = new Cell[_width, _heigth];
            CreateGrid(Vector3.zero);
            _currentBrush = Instantiate(_brush, grid.GetCellWorldPosition(0, 0), Quaternion.identity);
            _currentBrush.Coords = new Vector2Int(0, 0);
        }

        // Update is called once per frame
        void Update()
        {
            if(swipeControl!=null)
            {
                swipeControl.OnUpdate();
            }

        }


        private void CreateGrid(Vector3 originpos)
        {
            for (int x = 0; x < grid.GridArray.GetLength(0); x++)
            {
                for (int y = 0; y < grid.GridArray.GetLength(1); y++)
                {
                   cells[x,y]= createCell(x,y,originpos);
                 
                }
            }
        }

        private Cell createCell(int x,int y,Vector3 originpos)
        {
            Cell cell = Instantiate(block);
            cell.Coords = new Vector2Int(x, y);
            cell.transform.localScale = new Vector3(_cellSize, 0.25f, _cellSize);
            cell.transform.position =originpos + grid.GetCellWorldPosition(x, y);
            return cell;

        }

       public void MoveBrush(Swipe direction)
        {
            Vector2Int newcoords = grid.GetCellXZBySwipe(_currentBrush.Coords.x,_currentBrush.Coords.y,direction);

            if(newcoords != new Vector2Int(-1,-1))
            {
                Vector3 finalPos = grid.GetCellWorldPosition(newcoords.x, newcoords.y);

                if (!ConnectionAlreadyDone(_currentBrush.Coords, newcoords, true))
                {

                    inProgressPattern.Add(new Connection(_currentBrush.Coords, newcoords));
                    cells[_currentBrush.Coords.x, _currentBrush.Coords.y].CellCenterPaint.gameObject.SetActive(true);
                    // cells[currentBrush.currentCoords.x, currentBrush.currentCoords.y].CellCenterPaint.material.color = colors[GameManager.currentLevel % colors.Length];
                    LinePaintScript linePaint = Instantiate(linePaintPrefab, new Vector3(0, 0.2f, 0), Quaternion.identity);
                    linePaint.SetRendererPosition(_currentBrush.transform.position + new Vector3(0, 0.2f, 0),
                        finalPos + new Vector3(0, 0.2f, 0));
                    linePaint.SetConnectedCoords(_currentBrush.Coords, newcoords);
                    connectedLinePaints.Add(linePaint);

                }
                else
                {
                    RemoveConnectLinePaint(_currentBrush.Coords, newcoords);
                }

                _currentBrush.transform.position = finalPos;
                _currentBrush.Coords = newcoords;
            }




        }

        private bool ConnectionAlreadyDone(Vector2Int startCoord, Vector2Int endCoord, bool removeItem)
        {
            bool connected = false;
            for (int i = 0; i < inProgressPattern.Count; i++)
            {
                if (inProgressPattern[i].StartCoords == startCoord && inProgressPattern[i].EndCoords == endCoord ||
                    inProgressPattern[i].EndCoords == startCoord && inProgressPattern[i].StartCoords == endCoord)
                {
                    if (removeItem)
                    {
                        inProgressPattern.RemoveAt(i);
                    }

                    connected = true;
                }
            }

            return connected;
        }

        private void RemoveConnectLinePaint(Vector2Int startCoord, Vector2Int endCoord)
        {
            for (int i = 0; i < connectedLinePaints.Count; i++)
            {
                if (connectedLinePaints[i].StartCoord == startCoord && connectedLinePaints[i].EndCoord == endCoord ||
                    connectedLinePaints[i].EndCoord == startCoord && connectedLinePaints[i].StartCoord == endCoord)
                {
                    LinePaintScript line = connectedLinePaints[i];
                    connectedLinePaints.RemoveAt(i);
                    Destroy(line.gameObject);

                    cells[endCoord.x, endCoord.y].CellCenterPaint.gameObject.SetActive(false);
                    return;
                }
            }
        }



    }
}
