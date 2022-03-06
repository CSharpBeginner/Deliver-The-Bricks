using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(Rigidbody))]
public class Brick : MonoBehaviour
{
    [SerializeField] private float _step;//10
    [SerializeField] private float _waitingTime;//0.1

    private BoxCollider _boxCollider;
    private Rigidbody _rigidbody;

    public event UnityAction Collide;

    private void Awake()
    {
        _boxCollider = GetComponent<BoxCollider>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Obstacle>(out Obstacle obstacle))
        {
            _boxCollider.isTrigger = false;
            _rigidbody.useGravity = true;
            _rigidbody.isKinematic = false;
            transform.SetParent(null);
            Collide?.Invoke();
        }
    }

    public void Fall()
    {
        gameObject.SetActive(true);
        StartCoroutine(Falling());
    }

    private IEnumerator Falling()
    {
        var waiting = new WaitForSeconds(_waitingTime);
        float progress = 0f;
        float startHeigth = transform.localPosition.y;
        float target = 0f;

        while (progress < 1)
        {
            float height = Mathf.Lerp(startHeigth, target, progress);
            transform.localPosition = new Vector3(transform.localPosition.x, height, transform.localPosition.z);
            progress += _step * Time.deltaTime;
            yield return null;
        }

        transform.localPosition = new Vector3(transform.localPosition.x, target, transform.localPosition.z);
        yield return waiting;
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }

    public void Fly(Transform target, float step, float waitingTime)
    {
        gameObject.SetActive(true);
        StartCoroutine(Flying(target, step, waitingTime));
    }

    private IEnumerator Flying(Transform target, float step, float waitingTime)
    {
        var waiting = new WaitForSeconds(waitingTime);
        float progress = 0f;
        Vector3 startPosition = transform.position;

        while (progress < 1)
        {
            transform.position = Vector3.Lerp(startPosition, target.position, progress);
            progress += step * Time.deltaTime;
            yield return null;
        }

        transform.position = target.position;
        yield return waiting;
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }
}
