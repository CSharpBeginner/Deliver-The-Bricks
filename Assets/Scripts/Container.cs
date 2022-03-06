using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Container : MonoBehaviour
{
    //[SerializeField] private Road _road;
    [SerializeField] private float _flyingStep;//10
    [SerializeField] private float _flyingWaitingTime;//0.1
    [SerializeField] private float _timeBeforeFalling;//0.2
    [SerializeField] private int _appearingBricksCount;//3
    [SerializeField] private int _fallingBricksCount;//3
    [SerializeField] private Brick _brick;
    [SerializeField] private Floor[] _floorTypes;
    [SerializeField] private Vector2Int _basis;//убрать перед сдачей
    [SerializeField] private int _height;//как вариант переместить в базис
    [SerializeField] private Pool _pool;
    [SerializeField] private float _offset;

    private List<Floor> _floors;
    private BoxCollider _boxCollider;//убрать перед сдачей
    private Coroutine _losing;

    private void Awake()
    {
        _floors = new List<Floor>();
        _boxCollider = GetComponent<BoxCollider>();//убрать перед сдачей
        _boxCollider.size = ComputeSize(_brick.transform.localScale);//убрать перед сдачей
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<Obstacle>(out Obstacle obstacle))
        {
            //добавить cooldown?
            Queue<Place> newFillingPlaces = GetNewFillingPlaces();
            DeleteEmptyFloors();
            StartCoroutine(Falling(newFillingPlaces, _timeBeforeFalling, _fallingBricksCount));
        }
    }

    public List<Brick> LoseBricks(int count)
    {
        List<Brick> bricks = new List<Brick>(count);

        while (_floors.Count != 0)
        {
            Floor floor = _floors[_floors.Count - 1];
            int placesCount = floor.Places.Count;

            for (int j = 0; j < placesCount; j++)
            {
                if (floor.Places[j].Brick != null)
                {
                    bricks.Add(floor.Places[j].Brick);
                    floor.Places[j].Free();
                }

                if (j == floor.Places.Count - 1)
                {
                    _floors.Remove(floor);
                }

                if (bricks.Count == count)
                {
                    return bricks;
                }
            }
        }

        return bricks;
    }

    private Vector3 ComputeSize(Vector3 elementScale)//убрать перед сдачей
    {
        return new Vector3((elementScale.x + _offset) * _basis.x + elementScale.z, _height, (elementScale.z + _offset) * _basis.y + elementScale.x);
    }

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

    private IEnumerator Falling(Queue<Place> endPlaces, float timeBeforeFalling, int countPerFrame)
    {
        yield return new WaitForSeconds(timeBeforeFalling);

        while (endPlaces.Count != 0)
        {
            int fallingBricksCount = Mathf.Clamp(endPlaces.Count, 0, countPerFrame);

            for (int i = 0; i < fallingBricksCount; i++)
            {
                Place place = endPlaces.Dequeue();
                Brick brick = place.Brick;

                if (brick != null)
                {
                    brick.Fly(place.transform, _flyingStep, _flyingWaitingTime);
                }
            }

            yield return null;
        }
    }

    public void Fill(int count)
    {
        Queue<Brick> bricks = new Queue<Brick>(count);

        while (count > 0)
        {
            Floor floorType = _floorTypes[_floors.Count % _floorTypes.Length];
            Floor floor = Instantiate(floorType, transform);
            floor.transform.localPosition = new Vector3(0, (_brick.transform.localScale.y + _offset) * _floors.Count, 0);
            //floor.transform.localScale = 
            //floor.transform.localPosition = new Vector3(0, (_brick.transform.localScale.y + _offset) * transform.lossyScale.y* _floors.Count, 0) ;
            _floors.Add(floor);
            int bricksCountInFloor = Mathf.Clamp(count, 0, floor.Places.Count);
            count -= bricksCountInFloor;

            for (int j = 0; j < bricksCountInFloor; j++)
            {
                Brick brick = Instantiate(_brick);
                floor.Places[j].SetBrick(brick);
                brick.transform.localRotation = Quaternion.identity;
                brick.transform.localPosition = Vector3.zero;
                brick.transform.localScale = _brick.transform.localScale;
                bricks.Enqueue(brick);
            }
        }

        StartCoroutine(Appear(bricks, _appearingBricksCount));
    }

    private static IEnumerator Appear(Queue<Brick> bricks, int countPerFrame)
    {
        while (bricks.Count != 0)
        {
            int appearingBricksCount = Mathf.Clamp(bricks.Count, 0, countPerFrame);

            for (int i = 0; i < appearingBricksCount; i++)
            {
                Brick brick = bricks.Dequeue();
                brick.gameObject.SetActive(true);
            }

            yield return null;
        }
    }
}
