using System;
using System.Collections.Generic;
using UnityEngine;

public class Container : MonoBehaviour
{
    [SerializeField] private Brick _prefab;
    [SerializeField] private Vector2Int _basis;

    [SerializeField] private int _height;//как вариант переместить в базис
    private int _placesInFloor;
    private List<Floor> _floors;
    private BoxCollider _boxCollider;

    private void Awake()
    {
        _floors = new List<Floor>();
        _placesInFloor = (_basis.x + _basis.y) * 2;
        _boxCollider = GetComponent<BoxCollider>();
        _boxCollider.size = new Vector3(_prefab.transform.localScale.x * _basis.x + _prefab.transform.localScale.z, _height, _prefab.transform.localScale.z * _basis.y + _prefab.transform.localScale.x);
    }

    private void Start()
    {
        Fill(120);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<Obstacle>(out Obstacle obstacle))
        {
            //TryFall();
        }
    }

    private void GetEndPlace(Place current)//¬ообще все кирпичи могут получать новое место. Ћибо только падающие
    {
        Place end = current;
        //if ()
    }

    private void TryFall()//TryGetEmptyFloorNumber
    {
        if (_floors.Count > 1)
        {
            for (int i = 1; i < _floors.Count; i++)
            {
                //TryGetBrick
                for (int j = 0; j < _floors[i].Places.Count; j++)
                {
                    if (_floors[i].Places[j].Brick != null)
                    {
                        //GetEndPlace
                        Place end = _floors[i].Places[j];
                        for (int k = i - 1; k >= 0; k--)
                        {
                            //Check1
                            if (_floors[k].Places[j - _basis.y] == null && _floors[k].Places[j - _basis.y + 1] == null)
                            {
                                end = _floors[k].Places[j - _basis.y];//а еще будут ошибки дл€ боковых элементов слева
                            }
                            else
                            {
                                //break;
                            }
                            //Check2 - она цепочкой должна идти за Check1
                            if (_floors[k].Places[j] == null)
                            {
                                end = _floors[k].Places[j - _basis.y];
                            }
                            else
                            {
                                //break;
                            }

                        }
                        //if (_floors[i].Places[j].Brick)
                    }
                }
            }
        }

        for (int i = 0; i < _floors.Count; i++)
        {
            if (_floors[i].IsEmpty())
            {
                //»щем не пустой этаж, обрушаем, если не находим надо удалить пустые
                //»ли обрушаем. в любом случае потом удал€ем пустые этажи
            }
        }
        foreach (Floor floor in _floors)
        {
            if (floor.IsEmpty())
            {
                //»щем не пустой этаж, обрушаем, если не находим надо удалить пустые
                //»ли обрушаем. в любом случае потом удал€ем пустые этажи
            }
        }
    }

    private void Check1()
    {

    }

    private void TryFallAllAbove(int floorNumber)
    {
        for (int i = floorNumber + 1; i < _floors.Count; i++)
        {
            //_floors[i].Fall();
            //«апускаем дл€ каждого этажа метод обрушени€.  аждый этаж последовательно находит и дает команду на обрушение кирпичу.
            //ƒумаю одновременно падает весь этаж
            //ѕолучить кирпич - не нужно дай команду, место само решит кидать кирпич или нет. ќбрушить, давай следующий(через некоторое врем€)!
            if (_floors[i].IsEmpty())
            {
                //»щем не пустой этаж, обрушаем, если не находим надо удалить пустые
                //»ли обрушаем. в любом случае потом удал€ем пустые этажи
            }
        }
    }

    //private Place GetNewPlace()
    //{

    //}

    private void Fill(int count)
    {
        int floorsCount = (count - 1) / _placesInFloor + 1;
        int startValue = _floors.Count;

        for (int i = startValue; i < startValue + floorsCount; i++)//а сработает ли _floors.Count + floorsCount ?
        {
            Floor floor = new Floor(_basis, i, _prefab.transform.localScale);
            _floors.Add(floor);
            FillFloor(floor, ref count);
        }
    }

    private void FillFloor(Floor floor, ref int count)
    {
        for (int i = 0; i < _placesInFloor; i++)
        {
            if (count != 0)
            {
                Brick newBrick = Instantiate(_prefab, transform);
                newBrick.transform.rotation = floor.Places[i].Rotation;
                newBrick.transform.localPosition = floor.Places[i].Position;
                floor.Places[i].SetBrick(newBrick);
                count--;
            }
        }
    }
}
