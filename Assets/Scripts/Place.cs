using UnityEngine;

public class Place : MonoBehaviour
{
    private Brick _brick;

    public Brick Brick => _brick;

    private void OnDisable()
    {
        if (_brick != null)
        {
            Free();
        }
    }

    public void SetBrick(Brick brick)
    {
        _brick = brick;
        brick.transform.parent = gameObject.transform;
        _brick.Collide += Free;
    }

    public void Free()
    {
        _brick.Collide -= Free;
        _brick = null;
    }
}
