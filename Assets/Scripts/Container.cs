using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Container : MonoBehaviour
{
    [SerializeField] private float _timeBetweenFilling;//0.01
    [SerializeField] private float _timeBetweenFalling;//0.01
    [SerializeField] private Brick _prefab;
    [SerializeField] private Vector2Int _basis;
    [SerializeField] private int _height;//��� ������� ����������� � �����
    [SerializeField] private Pool _pool;

    private int _placesInFloor;
    private List<Floor> _floors;
    private BoxCollider _boxCollider;
    //private FloorType[] _floorTypes;

    private void Awake()
    {
        _floors = new List<Floor>();
        _placesInFloor = (_basis.x + _basis.y) * 2;
        _boxCollider = GetComponent<BoxCollider>();
        _boxCollider.size = new Vector3(_prefab.transform.localScale.x * _basis.x + _prefab.transform.localScale.z, _height, _prefab.transform.localScale.z * _basis.y + _prefab.transform.localScale.x);
        //_floorTypes = new FloorType[2];
        //_floorTypes[0] = new FloorType(_basis, Rectangle.Corner.LeftBottom, false);
        //_floorTypes[1] = new FloorType(_basis, Rectangle.Corner.LeftTop, true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<Obstacle>(out Obstacle obstacle))
        {
            //�������� cooldown, ����� ����� ������������� ���������� ������ ���� ���
            Queue<Place> newFillingPlaces = GetNewFillingPlaces();
            DeleteEmptyFloors();
            StartCoroutine(Falling(newFillingPlaces));//�������� ��������
        }
    }

    private void DeleteEmptyFloors()
    {
        while (_floors.Count != 0 && _floors[_floors.Count - 1].IsEmpty())
        {
            _floors.Remove(_floors[_floors.Count - 1]);
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

    private bool TryFindPlaceBelow(Place firstPlaceBelow, Place secondPlaceBelow, out Place placeBelow)
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

    private bool TryFindPlaceBelow(int fallingFloorNumber, int checkingFloorNumber, int placeNumber, out Place placeBelow)//���������� (�������������) ��� ����� (� ���� ������ ����� 2 ���� ����� � ����������� �������������, ��� ��� ����� ������ )
                                                                                                                          //��� ����, ����� ������ �������� ������ ����� �� ������������ ������  
    {
        if ((fallingFloorNumber - checkingFloorNumber) % 2 == 1)
        {
            Place firstPlace;
            Place secondPlace;

            if (checkingFloorNumber % 2 == 0)//�����, ������� ������� ��� �����
            {
                firstPlace = _floors[checkingFloorNumber].Places[GetIndexInCyclicArray(_floors[checkingFloorNumber].Places.Count, placeNumber + _basis.y)];
                secondPlace = _floors[checkingFloorNumber].Places[GetIndexInCyclicArray(_floors[checkingFloorNumber].Places.Count, placeNumber + _basis.y - 1)];
            }
            else
            {
                firstPlace = _floors[checkingFloorNumber].Places[GetIndexInCyclicArray(_floors[checkingFloorNumber].Places.Count, placeNumber - _basis.y)];
                secondPlace = _floors[checkingFloorNumber].Places[GetIndexInCyclicArray(_floors[checkingFloorNumber].Places.Count, placeNumber - _basis.y + 1)];
            }

            return TryFindPlaceBelow(firstPlace, secondPlace, out placeBelow);
        }
        else
        {
            if (_floors[checkingFloorNumber].Places[placeNumber].Brick == null)
            {
                placeBelow = _floors[checkingFloorNumber].Places[placeNumber];
                return true;
            }
            else
            {
                placeBelow = null;
                return false;
            }
        }
    }

    private Queue<Place> GetNewFillingPlaces()
    {
        Queue<Place> newFillingPlaces = new Queue<Place>();

        for (int i = 1; i < _floors.Count; i++)
        {
            Stack<Place> places = new Stack<Place>();
            Stack<Brick> bricks = new Stack<Brick>();

            for (int j = 0; j < _floors[i].Places.Count; j++)
            {
                Place currentPlace = _floors[i].Places[j];

                if (currentPlace.Brick != null)
                {
                    Place newFillingPlace = null;

                    for (int k = i - 1; k >= 0; k--)
                    {
                        if (TryFindPlaceBelow(i, k, j, out Place placeBelow))
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

    private void FillPlaces(Stack<Brick> bricks, Stack<Place> newPlaces)
    {
        while (newPlaces.Count != 0)
        {
            Place place = newPlaces.Pop();
            Brick brick = bricks.Pop();
            if (place.Brick != null)
            {
                Debug.Log("Place have been already filled. How?");
            }
            place.SetBrick(brick);//brick �� null, �� ��� ���������� ������� brick=null
        }
    }

    private IEnumerator Falling(Queue<Place> endPlaces)
    {
        var waiting = new WaitForSeconds(_timeBetweenFalling);

        foreach (Place place in endPlaces)
        {
            if (place.Brick != null)
            {
                place.Brick.Fall(place.Position, place.Rotation);
            }

            yield return waiting;
        }
    }

    public void Fill(int count)
    {
        int floorsCount = (count - 1) / _placesInFloor + 1;
        int startIndex = _floors.Count;
        Queue<Brick> bricks = new Queue<Brick>(count);

        for (int i = startIndex; i < startIndex + floorsCount; i++)
        {
            Floor floor = new Floor(_basis, i, _prefab.transform.localScale);
            _floors.Add(floor);

            for (int j = 0; j < _placesInFloor; j++)
            {
                if (count != 0)
                {
                    Brick brick = Instantiate(_prefab, transform);
                    floor.Places[j].SetBrick(brick);
                    brick.transform.localRotation = floor.Places[j].Rotation;
                    brick.transform.localPosition = floor.Places[j].Position;
                    bricks.Enqueue(brick);
                    count--;
                }
                else
                {
                    break;
                }
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
