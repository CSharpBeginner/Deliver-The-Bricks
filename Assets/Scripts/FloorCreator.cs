using System.Collections.Generic;
using UnityEngine;

public class FloorCreator : MonoBehaviour
{
    [SerializeField] private Vector2Int _basis;
    [SerializeField] private float _offsetBetweenBricks;
    [SerializeField] private bool _hasBrickBefore;
    [ContextMenuItem("Create", nameof(Create))]
    [SerializeField] private Brick _prefab;

    private Vector2 _rectangle;

    private Vector2 Calculate(Vector3 elementScale)
    {
        return new Vector2((elementScale.x + _offsetBetweenBricks) * _basis.x + elementScale.z, (elementScale.z + _offsetBetweenBricks) * _basis.y + elementScale.x);
    }

    private void Create()
    {
        _rectangle = Calculate(_prefab.transform.localScale);
        Quaternion _rightRotation = Quaternion.Euler(0, 90, 0);
        int counter = 0;//Если я не создаю массива мне counter не нужен

        if (_hasBrickBefore == false)
        {
            Vector3 startPosition = new Vector3((-_rectangle.x + _prefab.transform.localScale.x) / 2, 0, (-_rectangle.y + _prefab.transform.localScale.z) / 2);
            SideCicle(startPosition, new Vector3((_prefab.transform.localScale.x + _offsetBetweenBricks), 0, 0), Quaternion.identity, _basis.x, ref counter);

            Vector3 second = new Vector3((_rectangle.x - _prefab.transform.localScale.z) / 2, 0, (-_rectangle.y + _prefab.transform.localScale.x) / 2);
            SideCicle(second, new Vector3(0, 0, (_prefab.transform.localScale.x + _offsetBetweenBricks)), _rightRotation, _basis.y, ref counter);

            Vector3 third = new Vector3((_rectangle.x - _prefab.transform.localScale.x) / 2, 0, (_rectangle.y - _prefab.transform.localScale.z) / 2);
            SideCicle(third, new Vector3(-(_prefab.transform.localScale.x + _offsetBetweenBricks), 0, 0), Quaternion.identity, _basis.x, ref counter);

            Vector3 fourth = new Vector3((-_rectangle.x + _prefab.transform.localScale.z) / 2, 0, (_rectangle.y - _prefab.transform.localScale.x) / 2);
            SideCicle(fourth, new Vector3(0, 0, -(_prefab.transform.localScale.x + _offsetBetweenBricks)), _rightRotation, _basis.y, ref counter);
        }
        else
        {
            Vector3 startPosition = new Vector3((-_rectangle.x + _prefab.transform.localScale.x) / 2 + _prefab.transform.localScale.z + _offsetBetweenBricks, 0, (-_rectangle.y + _prefab.transform.localScale.z) / 2);
            SideCicle(startPosition, new Vector3((_prefab.transform.localScale.x + _offsetBetweenBricks), 0, 0), Quaternion.identity, _basis.x, ref counter);

            Vector3 second = new Vector3((_rectangle.x - _prefab.transform.localScale.z) / 2, 0, (-_rectangle.y + _prefab.transform.localScale.x) / 2 + _prefab.transform.localScale.z + _offsetBetweenBricks);
            SideCicle(second, new Vector3(0, 0, (_prefab.transform.localScale.x + _offsetBetweenBricks)), _rightRotation, _basis.y, ref counter);

            Vector3 third = new Vector3((_rectangle.x - _prefab.transform.localScale.x) / 2 - _prefab.transform.localScale.z - _offsetBetweenBricks, 0, (_rectangle.y - _prefab.transform.localScale.z) / 2);
            SideCicle(third, new Vector3(-(_prefab.transform.localScale.x + _offsetBetweenBricks), 0, 0), Quaternion.identity, _basis.x, ref counter);

            Vector3 fourth = new Vector3((-_rectangle.x + _prefab.transform.localScale.z) / 2, 0, (_rectangle.y - _prefab.transform.localScale.x) / 2 - _prefab.transform.localScale.z - _offsetBetweenBricks);
            SideCicle(fourth, new Vector3(0, 0, -(_prefab.transform.localScale.x + _offsetBetweenBricks)), _rightRotation, _basis.y, ref counter);
        }
    }

    private void SideCicle(Vector3 startPosition, Vector3 translation, Quaternion rotation, int countInLine, ref int counter)
    {
        for (int i = 0; i < countInLine; i++)
        {
            Vector3 position = startPosition + translation * i;
            GameObject gameObject = new GameObject($"Place {counter}", typeof(Place));
            gameObject.transform.position = position;
            gameObject.transform.rotation = rotation;
            gameObject.transform.parent = transform;
            counter++;
        }
    }
}
