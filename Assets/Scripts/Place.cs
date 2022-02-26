using UnityEngine;

public class Place
{
    private Vector3 _position;
    private Quaternion _rotation;
    private Brick _brick;

    public Brick Brick => _brick;
    public Vector3 Position => _position;
    public Quaternion Rotation => _rotation;

    public Place(Vector3 position, Quaternion rotation)
    {
        _position = position;
        _rotation = rotation;
    }

    public void SetBrick(Brick brick)
    {
        if (_brick != null)
        {
            Debug.Log("not Free");
            Free();
        }

        _brick = brick;
        _brick.Collide += Free;
    }

    public void Free()
    {
        _brick.Collide -= Free;
        _brick = null;
    }

    ~Place()
    {
        if (_brick != null)
        {
            Free();
        }
    }
}
