using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorType
{
    private Vector2Int _basis;
    private Corner _startFrom;
    //private bool _isClockwise;
    private bool _hasBrickBefore;

    public Vector2Int Basis => _basis;
    public Corner StartCorner => _startFrom;
    public bool HasBrickBefore => _hasBrickBefore;

    public FloorType(Vector2Int basis, Corner startFrom, bool hasBrickBefore)
    {
        _basis = basis;
        _startFrom = startFrom;
        _hasBrickBefore = hasBrickBefore;
    }

    public enum Corner
    {
        LeftTop = 0,
        LeftBottom = 1,
        RightBottom = 2,
        RightTop = 3
    }

    public static Vector2 GetCornerCoordinate(Corner corner, Vector2 rightTopPosition)
    {
        switch (corner)
        {
            case Corner.LeftTop:
                return new Vector2(-rightTopPosition.x, rightTopPosition.y);
            case Corner.LeftBottom:
                return new Vector2(-rightTopPosition.x, -rightTopPosition.y);
            case Corner.RightBottom:
                return new Vector2(rightTopPosition.x, -rightTopPosition.y);
            case Corner.RightTop:
                return rightTopPosition;
        }

        return Vector2.zero;
    }

    public static Vector2 GetOffsetFromCorner(Corner corner, Vector2 offsetFromLeftBottomPosition)
    {
        Vector2 result = offsetFromLeftBottomPosition;

        switch (corner)
        {
            case Corner.LeftTop:
                result = new Vector2(offsetFromLeftBottomPosition.y, offsetFromLeftBottomPosition.x);
                break;
            case Corner.LeftBottom:
                result = offsetFromLeftBottomPosition;
                break;
            case Corner.RightBottom:
                result = new Vector2(offsetFromLeftBottomPosition.y, offsetFromLeftBottomPosition.x);
                break;
            case Corner.RightTop:
                result = offsetFromLeftBottomPosition;
                break;
        }

        return -FloorType.GetCornerCoordinate(corner, result);
    }
}
