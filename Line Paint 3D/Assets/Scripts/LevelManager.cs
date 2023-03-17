using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LinePaint
{
    public class LevelManager : MonoBehaviour
    {
        public Cell block;
        [SerializeField] private float _cellSize;
      
        [SerializeField] private CameraZoom _gameCamera;
        [SerializeField] private CameraZoom solutionCamera;
        [SerializeField] private BrushController _brush;
        [SerializeField] private LinePaintScript linePaintPrefab;
        [SerializeField] private List<LevelScriptableData> _levelScriptables;
        [SerializeField] private Vector3 _gridOriginPos;
        [SerializeField] private UIManager uiManager;

        private int _heigth;
        private int _width;
        private Grid grid;
        private SwipeController swipeControl;
        private BrushController _currentBrush;
        private List<Connection> inProgressPattern = new List<Connection>();
        private List<LinePaintScript> connectedLinePaints = new List<LinePaintScript>();
        
        private Cell[,] cells;
        

        // Start is called before the first frame update
        void Start()
        {
            GameManager.gameStatus = GameStatus.Playing;
            GameManager.currentLevel = PlayerPrefs.GetInt("CurrentLevel", 0);
            uiManager.LevelText.text = "Level " + (GameManager.currentLevel + 1);
            grid = new Grid();
            _width = _levelScriptables[GameManager.currentLevel].Width;
            _heigth = _levelScriptables[GameManager.currentLevel].Height;
            swipeControl = new SwipeController();
            swipeControl.SetLevelManager(this);
            CompleteBoard();
            grid.Initialize(_width, _heigth, _cellSize,Vector3.zero);
           
            _gameCamera.ZoomPerspectiveCamera(_width, _heigth);
            cells = new Cell[_width, _heigth];

            

            CreateGrid(Vector3.zero);
            //  _currentBrush = Instantiate(_brush, grid.GetCellWorldPosition(0, 0), Quaternion.identity);
            _currentBrush = Instantiate(_brush, grid.GetCellWorldPosition(_levelScriptables[GameManager.currentLevel].BrushStartCoords.x, _levelScriptables[GameManager.currentLevel].BrushStartCoords.y), Quaternion.identity);

           // _currentBrush.Coords = new Vector2Int(0, 0);

            _currentBrush.Coords = _levelScriptables[GameManager.currentLevel].BrushStartCoords;
            Vector3 brushStartPos = grid.GetCellWorldPosition(_levelScriptables[GameManager.currentLevel].BrushStartCoords.x, _levelScriptables[GameManager.currentLevel].BrushStartCoords.y);
            _currentBrush.transform.position = brushStartPos;
        }

        // Update is called once per frame
        void Update()
        {
            if(swipeControl!=null &&GameManager.gameStatus==GameStatus.Playing )
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

                if (_levelScriptables[GameManager.currentLevel].completePattern.Count <= inProgressPattern.Count)
                {
                    //Check for win
                    if (IsLevelComplete())
                    {
                       // SoundManager.Instance.PlayFx(FxType.Victory);
                        GameManager.gameStatus = GameStatus.Complete;
                        Debug.Log("level completed");
                        GameManager.currentLevel++;
                        if (GameManager.currentLevel > _levelScriptables.Count - 1)
                        {
                            GameManager.currentLevel = 0;
                        }
                        PlayerPrefs.SetInt("CurrentLevel", GameManager.currentLevel);
                      //  GameManager.totalDiamonds += 15;
                      //  PlayerPrefs.SetInt("TotalDiamonds", GameManager.totalDiamonds);

                        uiManager.LevelCompleted();
                    }

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

        private void CompleteBoard()
        {
              Vector3 offset = new Vector3((_levelScriptables[GameManager.currentLevel].Width - _cellSize) / 2, 5f, (_levelScriptables[GameManager.currentLevel].Height - _cellSize) / 2);
            // Vector3 gridOriginPos = solutionCamera.transform.position - offset;
            solutionCamera.transform.position += offset;
              solutionCamera.ZoomOrthographicSizeCamera(_levelScriptables[GameManager.currentLevel].Width, _levelScriptables[GameManager.currentLevel].Height);

            grid.Initialize(_width, _heigth,_cellSize,_gridOriginPos);

            for (int i = 0; i < _levelScriptables[GameManager.currentLevel].completePattern.Count; i++)
            {
                Vector3 startPos = /*gridOriginPos +*/ grid.GetCellWorldPosition(_levelScriptables[GameManager.currentLevel].completePattern[i].StartCoords.x,
                    _levelScriptables[GameManager.currentLevel].completePattern[i].StartCoords.y);

                Vector3 endPos = /*gridOriginPos +*/ grid.GetCellWorldPosition(_levelScriptables[GameManager.currentLevel].completePattern[i].EndCoords.x,
                    _levelScriptables[GameManager.currentLevel].completePattern[i].EndCoords.y);

                LinePaintScript linePaint = Instantiate(linePaintPrefab, new Vector3(0, 0.2f, 0), Quaternion.identity);
                linePaint.SetRendererPosition(startPos /*+ new Vector3(0, 0.2f, 0)*/,
                    endPos /*+ new Vector3(0, 0.2f, 0)*/);
            }
        }

        private bool IsLevelComplete()
        {
            //if player has done more connection than required we return false
            if (_levelScriptables[GameManager.currentLevel].completePattern.Count != inProgressPattern.Count)
            {
                return false;
            }

            for (int i = 0; i < _levelScriptables[GameManager.currentLevel].completePattern.Count; i++)
            {
                if (!ConnectionAlreadyDone(_levelScriptables[GameManager.currentLevel].completePattern[i].StartCoords, _levelScriptables[GameManager.currentLevel].completePattern[i].EndCoords, false))
                {
                    return false;
                }
            }

            return true;
        }


    }
}
