using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Container : MonoBehaviour
{
    [SerializeField] private Brick _prefab;
    [SerializeField] private Vector2Int _basis;
    [SerializeField] private int _height;//как вариант переместить в базис
    [SerializeField] private float _waitingTime;

    private int _placesInFloor;
    private List<Floor> _floors;
    private BoxCollider _boxCollider;
    private bool _isTwoPlacesBelow = true;
    private List<Place> _endPlaces;

    private void Awake()
    {
        _endPlaces = new List<Place>();
        _floors = new List<Floor>();
        _placesInFloor = (_basis.x + _basis.y) * 2;
        _boxCollider = GetComponent<BoxCollider>();
        //не забудь убрать число 3 из следующей строки
        _boxCollider.size = new Vector3(_prefab.transform.localScale.x * _basis.x + _prefab.transform.localScale.z, _height, _prefab.transform.localScale.z * _basis.y + _prefab.transform.localScale.x);
    }

    private void CreateRoad()
    {
        //Установить таргет позишн и ротатион(из update) - из игрока - центральной точки
        //А вокруг спавнить кирпичи
        //Переместить первый кирпич с последнего этажа
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<Obstacle>(out Obstacle obstacle))
        {
            TryFall();
            DeleteEmptyFloors();
        }
    }

    private void DeleteEmptyFloors()
    {
        while (_floors[_floors.Count - 1].IsEmpty())
        {
            _floors.Remove(_floors[_floors.Count - 1]);
        }
    }

    private Place GetEndPlace(Place currentPlace, Place firstPlaceBelow, Place secondPlaceBelow)
    {
        Place endPlace = currentPlace;

        if (firstPlaceBelow.Brick == null && secondPlaceBelow.Brick == null)
        {
            endPlace = firstPlaceBelow;
            _isTwoPlacesBelow = false;
        }
        else
        {
            _isTwoPlacesBelow = true;
        }

        return endPlace;
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

    private Place GetEndPlace(int floorNumber, int placeNumber, Place current)
    {
        Place end = current;

        if (_isTwoPlacesBelow)
        {
            Place firstPlace;
            Place secondPlace;

            if (floorNumber % 2 == 0)//Лучше, конечно создать тип этажа
            {
                firstPlace = _floors[floorNumber].Places[GetIndexInCyclicArray(_floors[floorNumber].Places.Count, placeNumber + _basis.y)];
                secondPlace = _floors[floorNumber].Places[GetIndexInCyclicArray(_floors[floorNumber].Places.Count, placeNumber + _basis.y - 1)];
            }
            else
            {
                firstPlace = _floors[floorNumber].Places[GetIndexInCyclicArray(_floors[floorNumber].Places.Count, placeNumber - _basis.y)];
                secondPlace = _floors[floorNumber].Places[GetIndexInCyclicArray(_floors[floorNumber].Places.Count, placeNumber - _basis.y + 1)];
            }

            end = GetEndPlace(current, firstPlace, secondPlace);
        }
        else
        {
            if (_floors[floorNumber].Places[placeNumber].Brick == null)
            {
                end = _floors[floorNumber].Places[placeNumber];
            }

            _isTwoPlacesBelow = true;
        }

        return end;
    }

    private void TryFall()//TryGetEmptyFloorNumber //Думай над переписыванием именно этого метода!
    {
        for (int i = 1; i < _floors.Count; i++)
        {
            Place[] newPlaces = new Place[_placesInFloor];

            for (int j = 0; j < _floors[i].Places.Count; j++)
            {
                if (_floors[i].Places[j].Brick != null)
                {
                    Place startPlace = _floors[i].Places[j];
                    Place previousPlace = _floors[i].Places[j];

                    for (int k = i - 1; k >= 0; k--)
                    {
                        Place endPlace = GetEndPlace(k, j, previousPlace);

                        if (endPlace == previousPlace)
                        {
                            break;
                        }
                        else
                        {
                            previousPlace = endPlace;
                        }
                    }

                    if (startPlace != previousPlace)
                    {
                        _endPlaces.Add(previousPlace);
                    }

                    newPlaces[j] = previousPlace;
                }
            }

            FallFloor(_floors[i], newPlaces);
        }

        StartCoroutine(Falling());
    }

    private void FallFloor(Floor floor, Place[] newPlaces)
    {
        for (int j = 0; j < floor.Places.Count; j++)
        {
            if (floor.Places[j].Brick != null)
            {
                Brick brick = floor.Places[j].Brick;
                floor.Places[j].Free();
                newPlaces[j].SetBrick(brick);
            }
        }
    }

    private IEnumerator Falling()
    {
        var waiting = new WaitForSeconds(_waitingTime);

        foreach (Place place in _endPlaces)
        {
            place.Brick.Fall(place.Position, place.Rotation);
            yield return waiting;
        }
    }

    public void Fill(int count)
    {
        int floorsCount = (count - 1) / _placesInFloor + 1;
        int startIndex = _floors.Count;

        for (int i = startIndex; i < startIndex + floorsCount; i++)
        {
            Floor floor = new Floor(_basis, i, _prefab.transform.localScale);
            _floors.Add(floor);
        }

        StartCoroutine(Filling(startIndex, count));
    }

    private IEnumerator Filling(int startFloor, int count)
    {
        var waiting = new WaitForSeconds(0.02f);

        for (int i = startFloor; i < _floors.Count; i++)
        {
            for (int j = 0; j < _placesInFloor; j++)
            {
                if (count != 0)
                {
                    Brick newBrick = Instantiate(_prefab, transform);
                    newBrick.transform.rotation = _floors[i].Places[j].Rotation;
                    newBrick.transform.localPosition = _floors[i].Places[j].Position;
                    _floors[i].Places[j].SetBrick(newBrick);
                    count--;
                }
                else
                {
                    break;
                }

                yield return waiting;
            }
        }
    }
}
