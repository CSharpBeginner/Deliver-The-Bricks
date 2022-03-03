using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pool : MonoBehaviour
{
    private Stack<Brick> _bricks;

    private void Awake()
    {
        _bricks = new Stack<Brick>();
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
