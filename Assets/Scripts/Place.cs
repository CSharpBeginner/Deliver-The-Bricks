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
        _brick = brick;
        _brick.Collide += Free;
    }

    public void Free()
    {
        _brick.Collide -= Free;//!!!Необходимо гарантировать отписку! В деструкторе?
        _brick = null;
    }
}
