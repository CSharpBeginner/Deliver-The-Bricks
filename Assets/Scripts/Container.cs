using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Container : MonoBehaviour
{
    [SerializeField] private Brick _prefab;
    [SerializeField] private Vector2Int _basis;

    private List<Place> _places;
    private int _placesInRow;
    private Vector2 _rectangle;
    private Quaternion _rightRotation;

    private void Awake()
    {
        _places = new List<Place>();
        _placesInRow = (_basis.x + _basis.y) * 2;
        _rectangle = Calculate();
        _rightRotation = Quaternion.Euler(0, 90, 0);
    }

    private void Start()
    {
        Fill(20);
    }

    private Vector2 Calculate()
    {
        return new Vector2(_prefab.transform.localScale.x * _basis.x + _prefab.transform.localScale.z, _prefab.transform.localScale.z * _basis.y + _prefab.transform.localScale.x);
    }

    private void Fill(int count)
    {
        int nextRow;

        if (_places.Count != 0)
        {
            nextRow = (_places.Count - 1) / _placesInRow + 1;
        }
        else
        {
            nextRow = 0;
        }

        while (count != 0)
        {
            if (nextRow % 2 == 0)
            {
                Vector3 startPosition = new Vector3((-_rectangle.x + _prefab.transform.localScale.x) / 2, nextRow * _prefab.transform.localScale.y, (-_rectangle.y + _prefab.transform.localScale.z) / 2);
                SideCicle(startPosition, new Vector3(_prefab.transform.localScale.x, 0, 0), Quaternion.identity, _basis.x, ref count);

                Vector3 second = new Vector3((_rectangle.x - _prefab.transform.localScale.z) / 2, nextRow * _prefab.transform.localScale.y, (-_rectangle.y + _prefab.transform.localScale.x) / 2);
                SideCicle(second, new Vector3(0, 0, _prefab.transform.localScale.x), _rightRotation, _basis.y, ref count);

                Vector3 third = new Vector3((_rectangle.x - _prefab.transform.localScale.x) / 2, nextRow * _prefab.transform.localScale.y, (_rectangle.y - _prefab.transform.localScale.z) / 2);
                SideCicle(third, new Vector3(-_prefab.transform.localScale.x, 0, 0), Quaternion.identity, _basis.x, ref count);

                Vector3 fourth = new Vector3((-_rectangle.x + _prefab.transform.localScale.z) / 2, nextRow * _prefab.transform.localScale.y, (_rectangle.y - _prefab.transform.localScale.x) / 2);
                SideCicle(fourth, new Vector3(0, 0, -_prefab.transform.localScale.x), _rightRotation, _basis.y, ref count);
            }
            else
            {
                Vector3 startPosition = new Vector3((-_rectangle.x + _prefab.transform.localScale.x) / 2 + _prefab.transform.localScale.z, nextRow * _prefab.transform.localScale.y, (-_rectangle.y + _prefab.transform.localScale.z) / 2);
                SideCicle(startPosition, new Vector3(_prefab.transform.localScale.x, 0, 0), Quaternion.identity, _basis.x, ref count);

                Vector3 second = new Vector3((_rectangle.x - _prefab.transform.localScale.z) / 2, nextRow * _prefab.transform.localScale.y, (-_rectangle.y + _prefab.transform.localScale.x) / 2 + _prefab.transform.localScale.z);
                SideCicle(second, new Vector3(0, 0, _prefab.transform.localScale.x), _rightRotation, _basis.y, ref count);

                Vector3 third = new Vector3((_rectangle.x - _prefab.transform.localScale.x) / 2 - _prefab.transform.localScale.z, nextRow * _prefab.transform.localScale.y, (_rectangle.y - _prefab.transform.localScale.z) / 2);
                SideCicle(third, new Vector3(-_prefab.transform.localScale.x, 0, 0), Quaternion.identity, _basis.x, ref count);

                Vector3 fourth = new Vector3((-_rectangle.x + _prefab.transform.localScale.z) / 2, nextRow * _prefab.transform.localScale.y, (_rectangle.y - _prefab.transform.localScale.x) / 2 - _prefab.transform.localScale.z);
                SideCicle(fourth, new Vector3(0, 0, -_prefab.transform.localScale.x), _rightRotation, _basis.y, ref count);
            }

            nextRow++;
        }
    }

    private void SideCicle(Vector3 startPosition, Vector3 translation, Quaternion rotation, int countInLine, ref int count)
    {
        for (int i = 0; i < countInLine; i++)
        {
            if (count != 0)
            {
                Brick newBrick = Instantiate(_prefab, transform);
                newBrick.transform.rotation = rotation;
                newBrick.transform.localPosition = startPosition + translation * i;
                count--;
            }
            else
            {
                break;
            }
        }
    }
}
