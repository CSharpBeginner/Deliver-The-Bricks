using UnityEngine;

public class Follower : MonoBehaviour
{
    [SerializeField] private Player _player;

    private Vector3 _offset;

    private void Awake()
    {
        _offset = transform.position - _player.transform.position;
    }

    private void Update()
    {
        transform.position = new Vector3(transform.position.x, _player.transform.position.y + _offset.y, _player.transform.position.z + _offset.z);
    }
}
