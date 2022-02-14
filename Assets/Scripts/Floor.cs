using System;
using System.Collections.Generic;
using UnityEngine;

public class Floor
{
    private Vector2Int _basis;
    private Place[] _places;
    private Vector2 _rectangle;
    //private int _emptyPlaces;

    //public Action Emptied;

    public IReadOnlyList<Place> Places => _places;

    public Floor(Vector2Int basis, int number, Vector3 elementScale)
    {
        _basis = basis;
        Create(number, elementScale);
        //_emptyPlaces = _places.Length;
        //Subsribe();
    }

    //private void Subsribe()
    //{
    //    foreach (Place place in _places)
    //    {
    //        place.Emptied += Free;
    //        place.Filled += Fill;
    //    }
    //}

    //private void Free()
    //{
    //    _emptyPlaces++;

    //    if (_emptyPlaces == _places.Length)
    //    {
    //        Emptied?.Invoke();
    //    }
    //}

    //private void Fill()
    //{
    //    _emptyPlaces--;
    //}

    //public void Fall()
    //{
    //    foreach (Place place in _places)
    //    {
    //        place.Fall();
    //    }
    //}

    private Vector2 Calculate(Vector3 elementScale)
    {
        return new Vector2(elementScale.x * _basis.x + elementScale.z, elementScale.z * _basis.y + elementScale.x);
    }

    private void Create(int number, Vector3 elementScale)
    {
        int length = (_basis.x + _basis.y) * 2;
        _places = new Place[length];
        _rectangle = Calculate(elementScale);
        Quaternion _rightRotation = Quaternion.Euler(0, 90, 0);
        int counter = 0;

        if (number % 2 == 0)
        {
            Vector3 startPosition = new Vector3((-_rectangle.x + elementScale.x) / 2, number * elementScale.y, (-_rectangle.y + elementScale.z) / 2);
            SideCicle(startPosition, new Vector3(elementScale.x, 0, 0), Quaternion.identity, _basis.x, ref counter);

            Vector3 second = new Vector3((_rectangle.x - elementScale.z) / 2, number * elementScale.y, (-_rectangle.y + elementScale.x) / 2);
            SideCicle(second, new Vector3(0, 0, elementScale.x), _rightRotation, _basis.y, ref counter);

            Vector3 third = new Vector3((_rectangle.x - elementScale.x) / 2, number * elementScale.y, (_rectangle.y - elementScale.z) / 2);
            SideCicle(third, new Vector3(-elementScale.x, 0, 0), Quaternion.identity, _basis.x, ref counter);

            Vector3 fourth = new Vector3((-_rectangle.x + elementScale.z) / 2, number * elementScale.y, (_rectangle.y - elementScale.x) / 2);
            SideCicle(fourth, new Vector3(0, 0, -elementScale.x), _rightRotation, _basis.y, ref counter);
        }
        else
        {
            Vector3 startPosition = new Vector3((-_rectangle.x + elementScale.x) / 2 + elementScale.z, number * elementScale.y, (-_rectangle.y + elementScale.z) / 2);
            SideCicle(startPosition, new Vector3(elementScale.x, 0, 0), Quaternion.identity, _basis.x, ref counter);

            Vector3 second = new Vector3((_rectangle.x - elementScale.z) / 2, number * elementScale.y, (-_rectangle.y + elementScale.x) / 2 + elementScale.z);
            SideCicle(second, new Vector3(0, 0, elementScale.x), _rightRotation, _basis.y, ref counter);

            Vector3 third = new Vector3((_rectangle.x - elementScale.x) / 2 - elementScale.z, number * elementScale.y, (_rectangle.y - elementScale.z) / 2);
            SideCicle(third, new Vector3(-elementScale.x, 0, 0), Quaternion.identity, _basis.x, ref counter);

            Vector3 fourth = new Vector3((-_rectangle.x + elementScale.z) / 2, number * elementScale.y, (_rectangle.y - elementScale.x) / 2 - elementScale.z);
            SideCicle(fourth, new Vector3(0, 0, -elementScale.x), _rightRotation, _basis.y, ref counter);
        }
    }

    private void SideCicle(Vector3 startPosition, Vector3 translation, Quaternion rotation, int countInLine, ref int counter)
    {
        for (int i = 0; i < countInLine; i++)
        {
            Vector3 position = startPosition + translation * i;
            Place place = new Place(position, rotation);
            _places[counter] = place;
            counter++;
        }
    }

    public bool IsEmpty()
    {
        foreach (Place place in _places)
        {
            if (place != null)
            {
                return false;
            }
        }

        return true;
    }
}