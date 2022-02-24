using System.Collections.Generic;
using UnityEngine;

public class Floor2
{
    private Vector2Int _basis;
    private Place[] _places;
    private Vector2 _rectangle;
    private FloorType _floorType;
    //private Rect _rect;
    private Vector3 _rightTop;

    private Vector3 RightTop => _rightTop;

    public IReadOnlyList<Place> Places => _places;

    public Floor2(FloorType floorType, Vector2Int basis, int number, Vector3 elementScale)
    {
        _floorType = floorType;
        _basis = basis;
        Create(number, elementScale, floorType);
    }

    private Vector3 CalculateRightTop(Vector3 elementScale, Vector2Int basis)
    {
        return new Vector3((elementScale.x * basis.x + elementScale.z) / 2, 0, (elementScale.z * basis.y + elementScale.x) / 2);
    }

    private Vector2 Calculate(Vector3 elementScale, Vector2 basis)
    {
        return new Vector2(elementScale.x * basis.x + elementScale.z, elementScale.z * basis.y + elementScale.x);
    }

    private void Create(int number, Vector3 elementScale, FloorType floorType)//номер этажа нужен, но теперь нужен еще и тип
    {
        //_rect = new Rect(Vector2.zero, Vector2.one);
        //_rect.
        int length = (_basis.x + _basis.y) * 2;//необходимо получить из типа этажа!
        _places = new Place[length];
        _rectangle = Calculate(elementScale, _basis);//“еперь можно получить из типа этажа
        Quaternion _rightRotation = Quaternion.Euler(0, 90, 0);//¬ернутьс€ к этому позже
        int counter = 0;

        if (number % 2 == 0)//здесь тип
        {
            //leftDownCorner = (-_rectangle + Offset)/ 2;
            //Vector3 startPosition = new Vector3((-_rectangle.width + elementScale.x) / 2, number * elementScale.y, (-_rectangle.y + elementScale.z) / 2);
            Vector2 elementScale2 = new Vector2(elementScale.x, elementScale.z);
            Vector2 startPosition2 = GetStartPosition(floorType, elementScale2, floorType.HasBrickBefore);
            Vector3 startPosition = new Vector3(startPosition2.x, number * elementScale.y, startPosition2.y);
            //Vector3 startPosition = new Vector3((-_rectangle.x + elementScale.x) / 2, number * elementScale.y, (-_rectangle.y + elementScale.z) / 2);
            SideCicle(startPosition, elementScale.x, 0, _basis.x, ref counter);

            Vector3 second = new Vector3((_rectangle.x - elementScale.z) / 2, number * elementScale.y, (-_rectangle.y + elementScale.x) / 2);
            SideCicle(second, elementScale.x, 90, _basis.y, ref counter);

            Vector3 third = new Vector3((_rectangle.x - elementScale.x) / 2, number * elementScale.y, (_rectangle.y - elementScale.z) / 2);
            SideCicle(third, elementScale.x, 180, _basis.x, ref counter);

            Vector3 fourth = new Vector3((-_rectangle.x + elementScale.z) / 2, number * elementScale.y, (_rectangle.y - elementScale.x) / 2);
            SideCicle(fourth, elementScale.x, 270, _basis.y, ref counter);
        }
        else
        {
            Vector3 startPosition = new Vector3((-_rectangle.x + elementScale.x) / 2 + elementScale.z, number * elementScale.y, (-_rectangle.y + elementScale.z) / 2);
            SideCicle(startPosition, elementScale.x, 0, _basis.x, ref counter);

            Vector3 second = new Vector3((_rectangle.x - elementScale.z) / 2, number * elementScale.y, (-_rectangle.y + elementScale.x) / 2 + elementScale.z);
            SideCicle(second, elementScale.x, 90, _basis.y, ref counter);

            Vector3 third = new Vector3((_rectangle.x - elementScale.x) / 2 - elementScale.z, number * elementScale.y, (_rectangle.y - elementScale.z) / 2);
            SideCicle(third, elementScale.x, 180, _basis.x, ref counter);

            Vector3 fourth = new Vector3((-_rectangle.x + elementScale.z) / 2, number * elementScale.y, (_rectangle.y - elementScale.x) / 2 - elementScale.z);
            SideCicle(fourth, elementScale.x, 270, _basis.y, ref counter);
        }
    }

    private Vector2 GetStartPosition(FloorType floorType, Vector2 elementScale, bool hasBrickBefor)
    {
        float offset = 0;

        if (hasBrickBefor)
        {
            offset = elementScale.y;
        }

        Vector2 offsetFromLeftBottomPosition = new Vector2(elementScale.x / 2 + offset, elementScale.y / 2);

        Vector2 cornerCoordinate = FloorType.GetCornerCoordinate(floorType.StartCorner, RightTop);
        Vector2 result = FloorType.GetOffsetFromCorner(floorType.StartCorner, offsetFromLeftBottomPosition);
        return result;
    }

    private void SideCicle(Vector3 startPosition, float step, float angle, int countInLine, ref int counter)
    {
        Quaternion rotation = Quaternion.Euler(0, angle, 0);
        Vector3 direction = rotation * Vector3.right;

        for (int i = 0; i < countInLine; i++)
        {
            Vector3 position = startPosition + direction * step * i;//вектор зависит от угла и направлени€ часовой стрелки
            Place place = new Place(position, rotation);
            _places[counter] = place;
            counter++;
        }
    }

    private void SideCicle(Vector3 corner, bool hasBrickBefor, Vector2 elementScale, Vector3 Sdirection, float angle, int countInLine, ref int counter)
    {
        float offset = 0;

        if (hasBrickBefor)
        {
            offset = elementScale.y;
        }

        Quaternion rotation = Quaternion.Euler(0, angle, 0);
        Vector3 direction = rotation * Vector3.right;
        Vector3 Vectoffset = new Vector3(elementScale.x / 2 + offset, 0, elementScale.y / 2);
        Vector3 startPosition = corner + rotation * Vectoffset;

        for (int i = 0; i < countInLine; i++)
        {
            Vector3 position = startPosition + direction * elementScale.x * i;
            Place place = new Place(position, rotation);
            _places[counter] = place;
            counter++;
        }
    }

    public bool IsEmpty()
    {
        foreach (Place place in _places)
        {
            if (place.Brick != null)
            {
                return false;
            }
        }

        return true;
    }
}
