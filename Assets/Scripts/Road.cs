using UnityEngine;

public class Road : MonoBehaviour
{
    [SerializeField] private Brick _prefab;
    [SerializeField] private float _maxRotationAngle;

    private float _halfOfLength;
    private float _halfOfWidth;
    private Quaternion _rightRotation;
    private Vector3 _lastPosition;
    private Quaternion _lastRotation;
    private float _squareOfDistance;
    private Vector3 _firstPositionOffset;
    private Vector3 _secondPositionOffset;
    private Vector3 _thirdPositionOffset;
    private Vector3 _fourthPositionOffset;

    private void Awake()
    {
        _halfOfLength = _prefab.transform.localScale.x / 2;
        _halfOfWidth = _prefab.transform.localScale.z / 2;
        _rightRotation = Quaternion.Euler(0, 90, 0);
        _squareOfDistance = _prefab.transform.localScale.x * _prefab.transform.localScale.x;
        _firstPositionOffset = new Vector3(-_prefab.transform.localScale.z * 1.5f, 0, _halfOfLength);
        _secondPositionOffset = new Vector3(-_halfOfWidth, 0, 0);
        _thirdPositionOffset = new Vector3(_halfOfWidth, 0, _halfOfLength);
        _fourthPositionOffset = new Vector3(_prefab.transform.localScale.z * 1.5f, 0, 0);
    }

    private void OnEnable()
    {
        _lastPosition = transform.position;
    }

    private void Update()
    {
        while ((transform.position - _lastPosition).sqrMagnitude > _squareOfDistance)
        {
            Vector3 direction = (transform.position - _lastPosition).normalized;
            _lastPosition += direction * _prefab.transform.localScale.x;
            _lastRotation = Quaternion.RotateTowards(_lastRotation, transform.rotation, _maxRotationAngle);
            SpawnLine();
        }
    }

    public void SpawnLine()
    {
        Quaternion rotation = _lastRotation * _rightRotation;
        Spawn(_firstPositionOffset, rotation);
        Spawn(_secondPositionOffset, rotation);
        Spawn(_thirdPositionOffset, rotation);
        Spawn(_fourthPositionOffset, rotation);
    }

    public void Spawn(Vector3 offset, Quaternion rotation)
    {
        Brick brick = Instantiate(_prefab, _lastPosition + offset, rotation);
        brick.gameObject.SetActive(true);
    }
}
