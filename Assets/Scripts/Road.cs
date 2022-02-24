using System.Collections.Generic;
using UnityEngine;

public class Road : MonoBehaviour
{
    [SerializeField] private Brick _prefab;
    [SerializeField] private float _maxRotationAngle;
    [SerializeField] private Builder _builder2;
    [SerializeField] private Grid _grid;
    [SerializeField] private float _verticalStep;

    private Quaternion _rightRotation;
    private Vector3 _lastPosition;
    private Quaternion _lastRotation;
    private float _squareOfDistance;
    private Vector3 _startPosition;
    private HashSet<Vector3Int> _filledCells;

    private void Awake()
    {
        _filledCells = new HashSet<Vector3Int>();
        _rightRotation = Quaternion.Euler(0, 90, 0);
        _squareOfDistance = _prefab.transform.localScale.x * _prefab.transform.localScale.x;
    }

    private void OnEnable()
    {
        _lastPosition = transform.position;
        _lastRotation = transform.rotation;
        _startPosition = _lastPosition;
        _builder2.transform.position = _lastPosition;
        _builder2.transform.rotation = _lastRotation;
        GetCellPosition();
    }

    private void Update()
    {
        while ((transform.position - _lastPosition).sqrMagnitude > _squareOfDistance)
        {
            Vector3 direction = (transform.position - _lastPosition).normalized;
            _lastPosition += direction * _verticalStep;
            _lastRotation = Quaternion.RotateTowards(_lastRotation, transform.rotation, _maxRotationAngle);
            _builder2.transform.position = _lastPosition;
            _builder2.transform.rotation = _lastRotation;
            GetCellPosition();
        }
    }

    private void GetCellPosition()
    {
        List<Vector3> positions = _builder2.GetPositions();

        foreach (Vector3 position in positions)
        {
            Vector3 localPosition = position - _startPosition;
            Vector3Int gridPositon = _grid.WorldToGridPosition(localPosition);

            if (_filledCells.Contains(gridPositon) == false)
            {
                _filledCells.Add(gridPositon);
                Spawn(gridPositon);
            }
        }
    }

    private float Calculate(Vector3 position)
    {
        float normalDistance = Mathf.Abs(position.x - _builder2.transform.position.x);
        float currentDistance = normalDistance / Mathf.Cos(Mathf.Deg2Rad * _lastRotation.eulerAngles.y);
        float difference = currentDistance - normalDistance;
        return difference;
    }

    private void Spawn(Vector3Int gridPositon)
    {
        Vector3 temp = _grid.GridToWorldPosition(gridPositon);
        Quaternion rotation = _lastRotation * _rightRotation;
        Vector3 position = _startPosition + temp;
        Brick brick = Instantiate(_prefab);
        brick.transform.position = position;
        brick.transform.rotation = rotation;
        Vector3 direction = brick.transform.right;
        float distance = Calculate(position);
        brick.transform.position -= direction * distance;
        brick.gameObject.SetActive(true);
    }
}
