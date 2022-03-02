using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Container : MonoBehaviour
{
    [SerializeField] private float _timeBetweenFilling;//0.01
    [SerializeField] private float _timeBetweenFalling;//0.01
    [SerializeField] private Brick _brick;
    [SerializeField] private Floor[] _floorTypes;
    [SerializeField] private Vector2Int _basis;
    [SerializeField] private int _height;//как вариант переместить в базис
    [SerializeField] private Pool _pool;
    [SerializeField] private float _offset;

    private List<Floor> _floors;
    private BoxCollider _boxCollider;

    private void Awake()
    {
        _floors = new List<Floor>();
        _boxCollider = GetComponent<BoxCollider>();
        Vector2 basis = Calculate(_brick.transform.localScale);
        _boxCollider.size = new Vector3(basis.x, _height, basis.y);
    }

    private Vector2 Calculate(Vector3 elementScale)
    {
        return new Vector2((elementScale.x + _offset) * _basis.x + elementScale.z, (elementScale.z + _offset) * _basis.y + elementScale.x);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<Obstacle>(out Obstacle obstacle))
        {
            //добавить cooldown, чтобы метод гарантировано запускался только один раз
            Queue<Place> newFillingPlaces = GetNewFillingPlaces();
            DeleteEmptyFloors();
            StartCoroutine(Falling(newFillingPlaces));//добавить задержку
            //StartCoroutine(Wait());
        }
    }

    //private IEnumerator Wait()
    //{
    //    yield return new WaitForSeconds(0.1f);
    //    Fall();
    //}

    //private void Fall()
    //{
    //    Queue<Place> newFillingPlaces = GetNewFillingPlaces();
    //    DeleteEmptyFloors();
    //    StartCoroutine(Falling(newFillingPlaces));//добавить задержку
    //}

    private void DeleteEmptyFloors()
    {
        while (_floors.Count != 0 && _floors[_floors.Count - 1].IsEmpty())
        {
            Floor floor = _floors[_floors.Count - 1];
            _floors.Remove(floor);
            Destroy(floor);
        }
    }

    private Place GetLowestEmptyPlaceBelow(int fallingFloorNumber, int placeNumber)
    {
        for (int i = 0; i < fallingFloorNumber; i++)
        {
            if (_floors[i].Places[placeNumber].Brick == null)
            {
                return _floors[i].Places[placeNumber];
            }
        }

        return null;
    }

    private Queue<Place> GetNewFillingPlaces()
    {
        Queue<Place> newFillingPlaces = new Queue<Place>();

        int startFloorIndex = 0;

        for (int i = 0; i < _floors.Count; i++)
        {
            startFloorIndex = i;

            if (_floors[i].IsEmpty())
            {
                break;
            }
        }

        for (int i = startFloorIndex + 1; i < _floors.Count; i++)
        {
            for (int j = 0; j < _floors[i].Places.Count; j++)
            {
                Place currentPlace = _floors[i].Places[j];

                if (currentPlace.Brick != null)
                {
                    Place newFillingPlace = GetLowestEmptyPlaceBelow(i, j);
                    newFillingPlace.SetBrick(currentPlace.Brick);
                    currentPlace.Free();
                    newFillingPlaces.Enqueue(newFillingPlace);
                }
            }
        }

        return newFillingPlaces;
    }

    private IEnumerator Falling(Queue<Place> endPlaces)//это нужно! круто переписать. места мне больше не нужны, нужны только кирпичи
    {
        var waiting = new WaitForSeconds(_timeBetweenFalling);

        foreach (Place place in endPlaces)
        {
            if (place.Brick != null)
            {
                place.Brick.Fall();
            }

            yield return waiting;
        }
    }

    public void Fill(int count)
    {
        int placesCount = 14;//magicNumber!!!!!!!
        int floorsCount = (count - 1) / placesCount + 1;
        int startIndex = _floors.Count;
        Queue<Brick> bricks = new Queue<Brick>(count);

        for (int i = startIndex; i < startIndex + floorsCount; i++)
        {
            Floor currentFloor = _floorTypes[i % _floorTypes.Length];
            Floor floor = Instantiate(currentFloor, transform);
            _floors.Add(floor);
            floor.transform.localPosition = new Vector3(0, (_brick.transform.localScale.y + _offset) * i, 0);
            int brickCountInFloor = Mathf.Clamp(count, 0, currentFloor.Places.Count);
            count -= brickCountInFloor;

            for (int j = 0; j < brickCountInFloor; j++)
            {
                Brick brick = Instantiate(_brick);
                floor.Places[j].SetBrick(brick);
                brick.transform.localRotation = Quaternion.identity;
                brick.transform.localPosition = Vector3.zero;
                brick.transform.localScale = _brick.transform.localScale;
                bricks.Enqueue(brick);
            }
        }

        StartCoroutine(Filling(bricks));
    }

    private IEnumerator Filling(Queue<Brick> bricks)
    {
        var waiting = new WaitForSeconds(_timeBetweenFilling);
        while (bricks.Count != 0)
        {
            Brick brick = bricks.Dequeue();
            brick.gameObject.SetActive(true);
            yield return waiting;
        }
    }
}
