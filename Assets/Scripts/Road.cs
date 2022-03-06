using System.Collections.Generic;
using UnityEngine;

public class Road : MonoBehaviour
{
    [SerializeField] private float _flyingStep;//10
    [SerializeField] private float _flyingWaitingTime;//0.1
    [SerializeField] private Transform _rightSpawnPoint;
    [SerializeField] private Transform _leftSpawnPoint;
    [SerializeField] private int _halfOfFlyingBricksCount;
    [SerializeField] private Container _container;
    [SerializeField] private Floor _bricksLine;
    [SerializeField] private Brick _brick;
    [SerializeField] private float _maxRotationAngle;

    private Vector3 _lastPosition;
    private Quaternion _lastRotation;

    private void OnEnable()
    {
        _lastPosition = transform.position;
        _lastRotation = transform.rotation;
    }

    private void Update()
    {
        Test();
    }

    private void Test()
    {
        while (transform.position.z > _lastPosition.z)
        {
            Vector3 direction = (transform.position - _lastPosition).normalized;
            _lastPosition += direction * _brick.transform.localScale.x * _container.transform.lossyScale.x;
            _lastRotation = Quaternion.RotateTowards(_lastRotation, transform.rotation, _maxRotationAngle);
            GetBrick();
        }
    }

    private void GetBrick()
    {
        Quaternion rotation = _lastRotation;
        Floor line = Instantiate(_bricksLine);
        line.transform.position = _lastPosition;
        line.transform.rotation = rotation;
        line.transform.localScale = _container.transform.lossyScale;//создаем места

        List<Brick> bricks = _container.LoseBricks(line.Places.Count);//метод будет возвращать потерянные кирпичи
        Build(bricks, line);//далее мы будем манипулировать с этими кирпичами
    }

    private void Build(List<Brick> bricks, Floor line)
    {
        for (int i = 0; i < bricks.Count; i++)
        {
            bricks[i].transform.parent = line.Places[i].transform;

            if (i < _halfOfFlyingBricksCount)
            {
                bricks[i].transform.position = _rightSpawnPoint.position;
                bricks[i].transform.rotation = Quaternion.identity;
                bricks[i].Fly(line.Places[i].transform, _flyingStep, _flyingWaitingTime);
            }
            else if (i < bricks.Count - _halfOfFlyingBricksCount)
            {
                bricks[i].gameObject.SetActive(true);
                bricks[i].transform.localPosition = Vector3.zero;
                bricks[i].transform.localRotation = Quaternion.identity;
            }
            else
            {
                bricks[i].transform.position = _leftSpawnPoint.position;
                bricks[i].transform.rotation = Quaternion.identity;
                bricks[i].Fly(line.Places[i].transform, _flyingStep, _flyingWaitingTime);
            }
        }
    }
}
