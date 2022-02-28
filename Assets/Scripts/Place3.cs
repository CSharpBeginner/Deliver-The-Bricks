using UnityEngine;

public class Place3 : MonoBehaviour
{
    private Brick _brick;

    public Brick Brick => _brick;

    private void OnDisable()
    {
        Free();
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
}
