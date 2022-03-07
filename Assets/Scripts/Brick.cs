using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(Rigidbody))]
public class Brick : MonoBehaviour
{
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

    public void Reset()
    {
        gameObject.SetActive(false);
        _boxCollider.isTrigger = true;
        _rigidbody.useGravity = false;
        _rigidbody.isKinematic = true;
    }

    public void Fall(Transform target, float step, float waitingTime)
    {
        gameObject.SetActive(true);
        StartCoroutine(Falling(target, step, waitingTime));
    }

    private IEnumerator Falling(Transform target, float step, float waitingTime)
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
