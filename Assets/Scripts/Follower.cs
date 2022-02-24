using UnityEngine;

public class Follower : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private bool _freezePositionX;
    [SerializeField] private bool _freezePositionY;
    [SerializeField] private bool _freezePositionZ;

    private Vector3 _offset;

    private void Awake()
    {
        _offset = transform.position - _player.transform.position;
    }

    private void Update()
    {
        transform.position = new Vector3(GetX(), GetY(), GetZ());
    }

    private float GetX()
    {
        if (_freezePositionX)
        {
            return transform.position.x;
        }
        else
        {
            return _player.transform.position.x + _offset.x;
        }
    }

    private float GetY()
    {
        if (_freezePositionY)
        {
            return transform.position.y;
        }
        else
        {
            return _player.transform.position.y + _offset.y;
        }
    }

    private float GetZ()
    {
        if (_freezePositionZ)
        {
            return transform.position.z;
        }
        else
        {
            return _player.transform.position.z + _offset.z;
        }
    }
}
