using System.Collections.Generic;
using UnityEngine;

public class Road : MonoBehaviour
{
    [SerializeField] private float _offset;
    [SerializeField] private float _flyingStep;
    [SerializeField] private float _flyingWaitingTime;
    [SerializeField] private float _maxRotationAngle;
    [SerializeField] private int _halfOfFlyingBricksCount;
    [SerializeField] private Transform _rightSpawnPoint;
    [SerializeField] private Transform _leftSpawnPoint;
    [SerializeField] private Container _container;
    [SerializeField] private Floor _bricksLine;
    [SerializeField] private Brick _brick;

    private Vector3 _lastPosition;
    private Quaternion _lastRotation;

    private void OnEnable()
    {
        _lastPosition = transform.position;
        _lastRotation = transform.rotation;
    }

    private void Update()
    {
        while (transform.position.z > _lastPosition.z)
        {
            BuildSection();
        }
    }

    private void BuildSection()
    {
        Vector3 direction = (transform.position - _lastPosition).normalized;
        _lastPosition += direction * (_brick.transform.localScale.x + _offset) * _container.transform.lossyScale.x;
        _lastRotation = Quaternion.RotateTowards(_lastRotation, transform.rotation, _maxRotationAngle);
        Floor line = Instantiate(_bricksLine, _lastPosition, _lastRotation);
        line.transform.localScale = _container.transform.lossyScale;
        List<Brick> bricks = _container.LoseBricks(line.Places.Count);
        Fill(line, bricks);
    }

    private void Fill(Floor line, List<Brick> bricks)
    {
        for (int i = 0; i < bricks.Count; i++)
        {
            bricks[i].transform.parent = line.Places[i].transform;
            bricks[i].transform.rotation = Quaternion.identity;

            if (i < _halfOfFlyingBricksCount)
            {
                Fall(bricks[i], _rightSpawnPoint.position, line.Places[i].transform);
            }
            else if (i < bricks.Count - _halfOfFlyingBricksCount)
            {
                Fall(bricks[i], line.Places[i].transform.position, line.Places[i].transform);
            }
            else
            {
                Fall(bricks[i], _leftSpawnPoint.position, line.Places[i].transform);
            }
        }
    }

    private void Fall(Brick brick, Vector3 startPosition, Transform target)
    {
        brick.transform.position = startPosition;
        brick.transform.rotation = Quaternion.identity;
        brick.Fall(target, _flyingStep, _flyingWaitingTime);
    }
}
