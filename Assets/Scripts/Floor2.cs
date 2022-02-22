using System.Collections.Generic;
using UnityEngine;

public class Floor2
{
    private Vector2Int _basis;
    private Place[] _places;
    private Vector2 _rectangle;
    private FloorType _floorType;

    public IReadOnlyList<Place> Places => _places;

    public Floor2(FloorType floorType, Vector2Int basis, int number, Vector3 elementScale)
    {
        _floorType = floorType;
        _basis = basis;
        Create(number, elementScale);
    }

    private Vector2 Calculate(Vector3 elementScale, Vector2 basis)
    {
        return new Vector2(elementScale.x * basis.x + elementScale.z, elementScale.z * basis.y + elementScale.x);
    }

    private void Create(int number, Vector3 elementScale)//номер этажа нужен, но теперь нужен еще и тип
    {
        int length = (_basis.x + _basis.y) * 2;//необходимо получить из типа этажа!
        _places = new Place[length];
        _rectangle = Calculate(elementScale, _basis);//“еперь можно получить из типа этажа
        Quaternion _rightRotation = Quaternion.Euler(0, 90, 0);//¬ернутьс€ к этому позже
        int counter = 0;

        if (number % 2 == 0)//здесь тип
        {
            //leftDownCorner = (-_rectangle + Offset)/ 2;
            //Vector3 startPosition = new Vector3((-_rectangle.width + elementScale.x) / 2, number * elementScale.y, (-_rectangle.y + elementScale.z) / 2);
            Vector3 startPosition = new Vector3((-_rectangle.x + elementScale.x) / 2, number * elementScale.y, (-_rectangle.y + elementScale.z) / 2);
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
