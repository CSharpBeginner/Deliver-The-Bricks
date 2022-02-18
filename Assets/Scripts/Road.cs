using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Road : MonoBehaviour
{
    [SerializeField] private Brick _prefab;

    private float _halfOfLength;
    private Quaternion _rightRotation;

    private void Awake()
    {
        _halfOfLength = _prefab.transform.localScale.x;
        _rightRotation = Quaternion.Euler(0, 90, 0);
    }

    private void OnEnable()
    {
        Spawn();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<Brick>(out Brick brick))
        {
            Spawn();
        }
    }

    public void Spawn()
    {
        Instantiate(_prefab, transform.position + Vector3.forward * _halfOfLength, _rightRotation);
    }
}
