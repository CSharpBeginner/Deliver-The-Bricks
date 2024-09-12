using System.Collections.Generic;
using UnityEngine;

public class Pool : MonoBehaviour
{
    private Stack<Brick> _bricks;

    private void Awake()
    {
        _bricks = new Stack<Brick>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Brick>(out Brick brick))
        {
            brick.Reset();
            _bricks.Push(brick);
        }
    }

    public bool TryGive(out Brick brick)
    {
        if (_bricks.Count != 0)
        {
            brick = _bricks.Pop();
            return true;
        }
        else
        {
            brick = null;
            return false;
        }
    }
}
