using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    [SerializeField] private MeshRenderer cellCenterPaint;
    [HideInInspector] public Vector2Int Coords;

    public MeshRenderer CellCenterPaint { get => cellCenterPaint; }
}
