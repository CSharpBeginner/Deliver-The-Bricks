using System.Collections.Generic;
using UnityEngine;

public class Builder : MonoBehaviour //Line
{
    [SerializeField] private float _width;
    [SerializeField] private float _horizontalStep;

    private float _halfOfWidth;

    private void Awake()
    {
        _halfOfWidth = _width / 2;
    }

    public List<Vector3> GetPositions()
    {
        List<Vector3> positions = new List<Vector3>();
        float distance = 0;

        Vector3 startPosition = transform.position + transform.right * _halfOfWidth;
        positions.Add(startPosition);

        while (distance != _width)
        {
            distance = Mathf.MoveTowards(distance, _width, _horizontalStep);
            Vector3 position = startPosition - transform.right * distance;
            positions.Add(position);
        }

        return positions;
    }
}
