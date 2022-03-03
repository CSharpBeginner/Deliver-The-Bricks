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
        List<Vector3> mainGridCenetrs = new List<Vector3>();

        foreach (Vector3 position in positions)
        {
            Vector3Int gridPositon = _grid.WorldToGridPosition(position);

            if (_filledCells.Contains(gridPositon) == false)
            {
                _filledCells.Add(gridPositon);
                Vector3 center = _grid.GridToWorldPosition(gridPositon);
                mainGridCenetrs.Add(center);
            }
        }

        GetLocalCellPosition(mainGridCenetrs);
    }

    private void GetLocalCellPosition(List<Vector3> oldCenters)
    {
        Quaternion originalRotation = Quaternion.Euler(0, _builder2.transform.eulerAngles.y, 0);
        Quaternion inverseRotation = Quaternion.Euler(0, -_builder2.transform.eulerAngles.y, 0);

        foreach (Vector3 oldCenter in oldCenters)
        {
            Vector3 localPosition = oldCenter - _builder2.transform.position;
            Vector3 rotatedLocalPosition = inverseRotation * localPosition;
            //дальше не верно
            Vector3Int gridPositon = _grid.WorldToGridPosition(rotatedLocalPosition);
            Debug.Log(gridPositon);
            Vector3 temp = _grid.GridToWorldPosition(gridPositon);
            Vector3 spawnPoint = _builder2.transform.position + originalRotation * temp;
            //Debug.Log("builder: " + _builder2.transform.position + "brick: " + originalRotation * temp);
            Spawn(spawnPoint);
        }
    }

    private void Spawn(Vector3 worldPositon)
    {
        Quaternion rotation = _lastRotation * _rightRotation;
        Brick brick = Instantiate(_prefab);
        brick.transform.position = worldPositon;
        brick.transform.rotation = rotation;
        brick.gameObject.SetActive(true);
        //Debug.Log("builder: " + _builder2.transform.position + "brick: " + brick.transform.position);
    }
}
