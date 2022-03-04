using UnityEngine;

public class ZeroFloor : MonoBehaviour
{
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private float _maxAngle;
    [SerializeField] private float _maxDeflection;
    [SerializeField] private float _deceleration;

    private float _startPositionX;
    private float _angle;
    private float _deflectionToAngleKoefficient;

    private void Awake()
    {
        _deflectionToAngleKoefficient = _maxAngle / _maxDeflection;
    }

    private void OnEnable()
    {
        _startPositionX = transform.position.x;
        _angle = 0;
    }

    private void Update()
    {
        float deflection = transform.position.x - _startPositionX;
        deflection = Mathf.Clamp(deflection, _startPositionX - _maxDeflection, _startPositionX + _maxDeflection);
        _startPositionX = Mathf.MoveTowards(_startPositionX, transform.position.x, _deceleration * Time.deltaTime);
        _startPositionX = Mathf.Clamp(_startPositionX, deflection - _maxDeflection, deflection + _maxDeflection);
        _angle = Mathf.MoveTowards(_angle, _deflectionToAngleKoefficient * deflection, _rotationSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Euler(0, 0, _angle);
    }
}
