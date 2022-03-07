using UnityEngine;

public class AboveWaterZone : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<Player>(out Player player))
        {
            player.ActivateRoad();
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<Player>(out Player player))
        {
            player.DeactivateRoad();
        }
    }
}
