using System;
using System.Collections;
using System.Collections.Generic;
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

    //private void OnDisable()
    //{
    //    Collide?.Invoke();
    //}

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Obstacle>(out Obstacle obstacle))
        {
            _boxCollider.isTrigger = false;
            _rigidbody.useGravity = true;
            _rigidbody.isKinematic = false;
            transform.SetParent(obstacle.transform);
            Collide?.Invoke();
        }
    }

    public void Fall(Vector3 targetPosition, Quaternion targetRotation)
    {
        gameObject.SetActive(true);
        StartCoroutine(Falling(targetPosition, targetRotation));
    }

    private IEnumerator Falling(Vector3 targetPosition, Quaternion targetRotation)
    {
        var waiting = new WaitForSeconds(_waitingTime);
        float progress = 0f;
        float startHeigth = transform.localPosition.y;
        float target = targetPosition.y;

        while (progress < 1)
        {
            float height = Mathf.Lerp(startHeigth, target, progress);
            transform.localPosition = new Vector3(transform.localPosition.x, height, transform.localPosition.z);
            progress += _step * Time.deltaTime;
            yield return null;
        }

        transform.localPosition = new Vector3(transform.localPosition.x, target, transform.localPosition.z);
        bool IsWait = true;

        while (IsWait)
        {
            IsWait = false;
            yield return waiting;
        }

        transform.localPosition = targetPosition;
        transform.localRotation = targetRotation;
    }
}
