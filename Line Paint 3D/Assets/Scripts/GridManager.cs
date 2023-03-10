using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public GameObject Cell;
    public float size = 1;

    // Start is called before the first frame update
    void Start()
    {
      //  GenreateGrid();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GenreateGrid()
    {
        for(int i=0;i<5;i++)
        {
            for(int j=0;j<5;j++)
            {
                //Instantiate(Cell, new Vector3(transform.position.x + i, 0, transform.position.z + j), Quaternion.identity);
                GameObject tile = Instantiate(Cell,transform.position,Quaternion.identity);
                float x = i * 2;
                float z = j * 2;
                tile.transform.position = new Vector3(x, 0, z);
            }
        }
    }

   public Vector3 getNearestCellPosition(Vector3 position)
    {
        position -= transform.position;
        int xcount = Mathf.RoundToInt(position.x/size);
        int ycount = Mathf.RoundToInt(position.y/size);
        int zcount = Mathf.RoundToInt(position.z/size);

        Vector3 result = new Vector3((float)xcount*size, (float)ycount*size,(float)zcount*size);
        result += transform.position;
        return result;

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        for (float x = 0; x < 40; x += size)
        {
            for (float z = 0; z < 40; z += size)
            {
                var point = getNearestCellPosition(new Vector3(x, 0f, z));
                Gizmos.DrawSphere(point, 0.1f);
            }

        }
    }


}
