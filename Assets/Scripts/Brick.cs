using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(Rigidbody))]
public class Brick : MonoBehaviour
{
    [SerializeField] private float _step;
    [SerializeField] private float _waitingTime;

    private BoxCollider _boxCollider;
    private Rigidbody _rigidbody;

    public Action Collide;

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
}
