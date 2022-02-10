using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
    [SerializeField] private Inputer _inputer;
    [SerializeField] private float _horizontalSpeed;
    [SerializeField] private float _verticalSpeed;
    [SerializeField] private float _horizontalMultiplier;

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
        Vector3 target = transform.position + Vector3.forward * _verticalSpeed * Time.deltaTime + Vector3.right * _horizontalDifference;//new Vector3(_horizontalDifference, 0, 0)
        _rigidbody.MovePosition(target);
    }
}