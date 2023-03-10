using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellPlacer : MonoBehaviour
{
    public GameObject Cell;
    public GridManager gm;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        PlaceCell();
    }

    void PlaceCell()
    {
        if(Input.GetMouseButtonDown(0))
        {
            RaycastHit hitInfo;
           Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            
            if (Physics.Raycast(ray, out hitInfo))
            {
                Debug.Log(hitInfo.point);
                var finalposition =  gm.getNearestCellPosition(hitInfo.point);
                Instantiate(Cell, finalposition, Quaternion.identity);
            }

        }
    }

}
