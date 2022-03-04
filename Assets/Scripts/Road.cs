using System.Collections.Generic;
using UnityEngine;

public class Road : MonoBehaviour
{
    [SerializeField] private Brick _prefab;
    [SerializeField] private float _maxRotationAngle;
    [SerializeField] private Line _line;
    [SerializeField] private Grid _grid;
    [SerializeField] private Grid2 _grid2;
    [SerializeField] private float _verticalStep;

    private Quaternion _rightRotation;
    private Vector3 _lastPosition;
    private Quaternion _lastRotation;
    private HashSet<Vector3Int> _filledCells;

    private void Awake()
    {
        _filledCells = new HashSet<Vector3Int>();
        _rightRotation = Quaternion.Euler(0, 90, 0);
    }

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
            _lastPosition += direction * _verticalStep;
            _lastRotation = Quaternion.RotateTowards(_lastRotation, transform.rotation, _maxRotationAngle);
            _line.transform.position = _lastPosition;
            //_builder.transform.rotation = _lastRotation;
            GetCellPosition();
        }
    }

    private void GetCellPosition()
    {
        List<Vector3> positions = _line.GetPositions();

        foreach (Vector3 position in positions)
        {
            Vector3Int gridPositon = _grid2.WorldToGridPosition(position);

            if (_filledCells.Contains(gridPositon) == false)
            {
                _filledCells.Add(gridPositon);
                Vector3 center = _grid2.GridToWorldPosition(gridPositon);
                Spawn(center);
            }
        }
    }

    private void Spawn(Vector3 worldPositon)
    {
        Quaternion rotation = _lastRotation * _rightRotation;
        Brick brick = Instantiate(_prefab);
        brick.transform.position = worldPositon;
        brick.transform.rotation = rotation;
        brick.gameObject.SetActive(true);
    }
}
