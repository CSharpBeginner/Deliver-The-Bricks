using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorType
{
    private Vector2Int _basis;
    private int _corner;
    //private bool _isClockwise;

    public FloorType(Vector2Int basis, int number, Vector3 elementScale)
    {
        _basis = basis;
        //Create(number, elementScale);
    }
}
