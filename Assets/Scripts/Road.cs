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

    private Vector3 CalculateTranslation(Vector3 position)
    {
        float angle = Vector3.Angle(Vector3.left, _lastPosition - position);
        Debug.Log("angle" + angle);
        if (angle > 90)
        {
            angle -= 90;
        }

        float hipotenus = _prefab.transform.localScale.z / 2 / Mathf.Cos(angle);
        float distance = hipotenus * (90 - _lastRotation.eulerAngles.y) / 90;
        Vector3 direction = (_lastPosition - position).normalized;
        return direction * distance;
    }

    private void Spawn(Vector3Int gridPositon)
    {
        Vector3 temp = _grid.GridToWorldPosition(gridPositon);
        Quaternion rotation = _lastRotation * _rightRotation;
        Vector3 position = _startPosition + temp;
        Debug.Log("_lastRotation" + _lastRotation.eulerAngles.y);
        Vector3 translation = (position - _lastPosition) * Mathf.Cos(Mathf.Deg2Rad * (90 - _lastRotation.eulerAngles.y));
        Debug.Log("_lastRotation" + _lastRotation.eulerAngles.y + "translation" + translation + "Cos" + Mathf.Cos(Mathf.Deg2Rad * (90 - _lastRotation.eulerAngles.y)));
        position -= translation;

        //Debug.Log("original position" + position);
        //Vector3 translation = CalculateTranslation(position);
        //position += translation;
        //Debug.Log("modifiyed position" + position);

        Brick brick = Instantiate(_prefab);
        brick.transform.position = position;
        brick.transform.rotation = rotation;
        brick.gameObject.SetActive(true);
    }
}
