using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
    [SerializeField] private Inputer _inputer;
    [SerializeField] private float _horizontalSpeed;
    [SerializeField] private float _verticalSpeed;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private float _horizontalMultiplier;
    [SerializeField] private GameObject _road;

    private Rigidbody _rigidbody;
    private Coroutine _currentCoroutine;
    private float _horizontalDifference;
    private float _startPositionX;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _horizontalDifference = 0;
    }

    private void OnEnable()
    {
        _inputer.TouchStarted += RememberPositionX;
        _inputer.NormalizedDifferenceChanged += OnNormalizedDifferenceChanged;
    }

    private void OnDisable()
    {
        _inputer.TouchStarted -= RememberPositionX;
        _inputer.NormalizedDifferenceChanged -= OnNormalizedDifferenceChanged;
    }

    public void ActivateRoad()
    {
        _road.SetActive(true);
    }

    public void DeactivateRoad()
    {
        _road.SetActive(false);
    }

    private void RememberPositionX()
    {
        _startPositionX = transform.position.x;
    }

    private void OnNormalizedDifferenceChanged(float value)
    {
        if (_currentCoroutine != null)
        {
            StopCoroutine(_currentCoroutine);
        }

        float target = _startPositionX + value * _horizontalMultiplier;
        _currentCoroutine = StartCoroutine(ComputeHorizontalDifference(target));
    }

    private IEnumerator ComputeHorizontalDifference(float target)
    {
        float nextPositionX = transform.position.x;

        while (nextPositionX != target)
        {
            nextPositionX = Mathf.MoveTowards(transform.position.x, target, _horizontalSpeed * Time.deltaTime);
            _horizontalDifference = nextPositionX - transform.position.x;
            yield return null;
        }

        if (nextPositionX == target)
        {
            _horizontalDifference = 0;
        }
    }

    private void Update()
    {
        Vector3 difference = Vector3.forward * _verticalSpeed * Time.deltaTime + Vector3.right * _horizontalDifference;
        Vector3 targetPosition = transform.position + difference;
        Quaternion targetRotation = Quaternion.LookRotation(difference);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
        _rigidbody.MovePosition(targetPosition);
    }
}
