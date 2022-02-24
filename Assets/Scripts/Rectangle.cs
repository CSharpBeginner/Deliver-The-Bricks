using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rectangle
{
    private Vector2 _position;
    private Vector2 _size;
    private Vector2 _halfOfSize;

    public enum Corner
    {
        LeftTop = 0,
        LeftBottom = 1,
        RightBottom = 2,
        RightTop = 3
    }

    public Rectangle(Vector2 position, Vector2 size)
    {
        _position = position;
        _size = size;
        _halfOfSize = _size / 2;
    }

    public Vector2 GetCornerCoordinate(Corner corner)
    {
        switch (corner)
        {
            case Corner.LeftTop:
                return new Vector2(-_halfOfSize.x, _halfOfSize.y);
            case Corner.LeftBottom:
                return new Vector2(-_halfOfSize.x, -_halfOfSize.y);
            case Corner.RightBottom:
                return new Vector2(_halfOfSize.x, -_halfOfSize.y);
            case Corner.RightTop:
                return _halfOfSize;
        }

        return Vector2.zero;
    }
}
