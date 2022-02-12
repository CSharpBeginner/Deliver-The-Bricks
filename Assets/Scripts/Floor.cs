using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : MonoBehaviour
{
    [SerializeField] private Vector2Int _basis;

    private Place[] _places;
    private Vector2 _rectangle;
    private Vector3 _brickSize;


    private void Awake()
    {
        int length = (_basis.x + _basis.y) * 2;
        _places = new Place[length];
        //_rectangle = Calculate();
    }

    private Vector2 Calculate(Vector3 elementScale)
    {
        return new Vector2(elementScale.x * _basis.x + elementScale.z, elementScale.z * _basis.y + elementScale.x);
    }

    private void Create(int nextRow, int count, Vector3 elementScale)
    {
        _rectangle = Calculate(elementScale);
        Quaternion _rightRotation = Quaternion.Euler(0, 90, 0);

        //int nextRow;

        //if (_places.Count != 0)
        //{
        //    nextRow = (_places.Count - 1) / _places.Length + 1;
        //}
        //else
        //{
        //    nextRow = 0;
        //}

        //while (count != 0)
        //{
        if (nextRow % 2 == 0)
        {
            Vector3 startPosition = new Vector3((-_rectangle.x + elementScale.x) / 2, 0f, (-_rectangle.y + elementScale.z) / 2);
            SideCicle(startPosition, new Vector3(elementScale.x, 0, 0), Quaternion.identity, _basis.x, ref count);
            //elementScale.z - это зависит от вращени€ объекта, лучше бы все перевести в зависимость от вращени€ и спавнить по направлению вращени€
            //Ќо все равно придетс€ устанавливать локальный старт
            //ћожно записать в пол€
            Vector3 second = new Vector3((_rectangle.x - elementScale.z) / 2, 0f, (-_rectangle.y + elementScale.x) / 2);
            SideCicle(second, new Vector3(0, 0, elementScale.x), _rightRotation, _basis.y, ref count);

            Vector3 third = new Vector3((_rectangle.x - elementScale.x) / 2, 0f, (_rectangle.y - elementScale.z) / 2);
            SideCicle(third, new Vector3(-elementScale.x, 0, 0), Quaternion.identity, _basis.x, ref count);

            Vector3 fourth = new Vector3((-_rectangle.x + elementScale.z) / 2, 0f, (_rectangle.y - elementScale.x) / 2);
            SideCicle(fourth, new Vector3(0, 0, -elementScale.x), _rightRotation, _basis.y, ref count);
        }
        else
        {
            Vector3 startPosition = new Vector3((-_rectangle.x + elementScale.x) / 2 + elementScale.z, 0f, (-_rectangle.y + elementScale.z) / 2);
            SideCicle(startPosition, new Vector3(elementScale.x, 0, 0), Quaternion.identity, _basis.x, ref count);

            Vector3 second = new Vector3((_rectangle.x - elementScale.z) / 2, 0f, (-_rectangle.y + elementScale.x) / 2 + elementScale.z);
            SideCicle(second, new Vector3(0, 0, elementScale.x), _rightRotation, _basis.y, ref count);

            Vector3 third = new Vector3((_rectangle.x - elementScale.x) / 2 - elementScale.z, 0f, (_rectangle.y - elementScale.z) / 2);
            SideCicle(third, new Vector3(-elementScale.x, 0, 0), Quaternion.identity, _basis.x, ref count);

            Vector3 fourth = new Vector3((-_rectangle.x + elementScale.z) / 2, 0f, (_rectangle.y - elementScale.x) / 2 - elementScale.z);
            SideCicle(fourth, new Vector3(0, 0, -elementScale.x), _rightRotation, _basis.y, ref count);
        }

        //nextRow++;
        //}
    }

    private void SideCicle(Vector3 startPosition, Vector3 translation, Quaternion rotation, int countInLine, ref int count)
    {
        for (int i = 0; i < countInLine; i++)
        {
            if (count != 0)
            {
                //Brick newBrick = Instantiate(_prefab, transform);
                //newBrick.transform.rotation = rotation;
                //newBrick.transform.localPosition = startPosition + translation * i;
                count--;
            }
            else
            {
                break;
            }
        }
    }
}
