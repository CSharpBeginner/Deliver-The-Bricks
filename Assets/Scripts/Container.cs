using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Container : MonoBehaviour
{
    [SerializeField] private float _timeBetweenFilling;//0.01
    [SerializeField] private float _timeBetweenFalling;//0.01
    [SerializeField] private Brick _brick;
    [SerializeField] private Floor3[] _floorTypes;
    [SerializeField] private Vector2Int _basis;
    [SerializeField] private int _height;//как вариант переместить в базис
    [SerializeField] private Pool _pool;
    [SerializeField] private float _offset;

    private List<Floor3> _floors3;
    private BoxCollider _boxCollider;

    private void Awake()
    {
        _floors3 = new List<Floor3>();
        _boxCollider = GetComponent<BoxCollider>();
        _boxCollider.size = new Vector3(_brick.transform.localScale.x * _basis.x + _brick.transform.localScale.z, _height, _brick.transform.localScale.z * _basis.y + _brick.transform.localScale.x);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<Obstacle>(out Obstacle obstacle))
        {
            //добавить cooldown, чтобы метод гарантировано запускался только один раз
            Queue<Place3> newFillingPlaces = GetNewFillingPlaces();
            DeleteEmptyFloors();
            StartCoroutine(Falling(newFillingPlaces));//добавить задержку
        }
    }

    private void DeleteEmptyFloors()
    {
        while (_floors3.Count != 0 && _floors3[_floors3.Count - 1].IsEmpty())
        {
            Floor3 floor = _floors3[_floors3.Count - 1];
            _floors3.Remove(floor);
            Destroy(floor);
        }
    }

    private int GetIndexInCyclicArray(int length, int outingOfRangeIndex)
    {
        if (outingOfRangeIndex < 0)
        {
            return length + outingOfRangeIndex % length;
        }
        else
        {
            return outingOfRangeIndex % length;
        }
    }

    private bool TryFindPlaceBelow(Place3 firstPlaceBelow, Place3 secondPlaceBelow, out Place3 placeBelow)
    {
        if (firstPlaceBelow.Brick == null && secondPlaceBelow.Brick == null)
        {
            placeBelow = firstPlaceBelow;
            return true;
        }
        else
        {
            placeBelow = null;
            return false;
        }
    }

    private bool TryFindPlaceBelow(int fallingFloorNumber, int checkingFloorNumber, int placeNumber, out Place3 placeBelow)//необходимо (относительный) тип этажа (в моем случае всего 2 типа этажа с поочередным расположением, так что можно забить )
                                                                                                                           //сам этаж, чтобы только получить нужное место по определенной логике  
    {
        if ((fallingFloorNumber - checkingFloorNumber) % 2 == 1)//переделать
        {
            Place3 firstPlace;
            Place3 secondPlace;

            if (checkingFloorNumber % 2 == 0)//Лучше, конечно создать тип этажа
            {
                firstPlace = _floors3[checkingFloorNumber].Places[GetIndexInCyclicArray(_floors3[checkingFloorNumber].Places.Count, placeNumber + _basis.y)];
                secondPlace = _floors3[checkingFloorNumber].Places[GetIndexInCyclicArray(_floors3[checkingFloorNumber].Places.Count, placeNumber + _basis.y - 1)];
            }
            else
            {
                firstPlace = _floors3[checkingFloorNumber].Places[GetIndexInCyclicArray(_floors3[checkingFloorNumber].Places.Count, placeNumber - _basis.y)];
                secondPlace = _floors3[checkingFloorNumber].Places[GetIndexInCyclicArray(_floors3[checkingFloorNumber].Places.Count, placeNumber - _basis.y + 1)];
            }

            return TryFindPlaceBelow(firstPlace, secondPlace, out placeBelow);
        }
        else
        {
            if (_floors3[checkingFloorNumber].Places[placeNumber].Brick == null)
            {
                placeBelow = _floors3[checkingFloorNumber].Places[placeNumber];
                return true;
            }
            else
            {
                placeBelow = null;
                return false;
            }
        }
    }

    private Queue<Place3> GetNewFillingPlaces()
    {
        Queue<Place3> newFillingPlaces = new Queue<Place3>();

        for (int i = 1; i < _floors3.Count; i++)
        {
            Stack<Place3> places = new Stack<Place3>();
            Stack<Brick> bricks = new Stack<Brick>();

            for (int j = 0; j < _floors3[i].Places.Count; j++)
            {
                Place3 currentPlace = _floors3[i].Places[j];

                if (currentPlace.Brick != null)
                {
                    Place3 newFillingPlace = null;

                    for (int k = i - 1; k >= 0; k--)
                    {
                        if (TryFindPlaceBelow(i, k, j, out Place3 placeBelow))
                        {
                            newFillingPlace = placeBelow;
                        }
                        else
                        {
                            break;
                        }
                    }

                    if (newFillingPlace != null)
                    {
                        if (newFillingPlace.Brick != null)
                        {
                            Debug.Log("TryFindPlaceBelow has a trouble");
                        }

                        places.Push(newFillingPlace);
                        bricks.Push(currentPlace.Brick);
                        currentPlace.Free();
                        newFillingPlaces.Enqueue(newFillingPlace);
                    }
                }
            }

            FillPlaces(bricks, places);
        }

        return newFillingPlaces;
    }

    private void FillPlaces(Stack<Brick> bricks, Stack<Place3> newPlaces)
    {
        while (newPlaces.Count != 0)
        {
            Place3 place = newPlaces.Pop();
            Brick brick = bricks.Pop();
            if (place.Brick != null)
            {
                Debug.Log("Place have been already filled");
            }
            place.SetBrick(brick);
        }
    }

    private IEnumerator Falling(Queue<Place3> endPlaces)
    {
        var waiting = new WaitForSeconds(_timeBetweenFalling);

        foreach (Place3 place in endPlaces)
        {
            if (place.Brick != null)
            {
                place.Brick.Fall(place.transform.position, place.transform.rotation);
            }

            yield return waiting;
        }
    }

    public void Fill(int count)
    {
        int placesCount = 14;//magicNumber!!!!!!!
        int floorsCount = (count - 1) / placesCount + 1;
        int startIndex = _floors3.Count;
        Queue<Brick> bricks = new Queue<Brick>(count);

        for (int i = startIndex; i < startIndex + floorsCount; i++)
        {
            Floor3 currentFloor = _floorTypes[i % _floorTypes.Length];
            Floor3 floor3 = Instantiate(currentFloor, transform);
            _floors3.Add(floor3);
            floor3.transform.localPosition = new Vector3(0, (_brick.transform.localScale.y + _offset) * i, 0);
            int brickCountInFloor = Mathf.Clamp(count, 0, currentFloor.Places.Count);
            count -= brickCountInFloor;

            for (int j = 0; j < brickCountInFloor; j++)
            {
                Brick brick = Instantiate(_brick, floor3.transform);
                brick.transform.rotation = floor3.Places[j].transform.rotation;
                brick.transform.position = floor3.Places[j].transform.position;
                floor3.Places[j].SetBrick(brick);
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
