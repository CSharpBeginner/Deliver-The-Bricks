using UnityEngine;

public class AboveWaterZone : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<Player>(out Player player))
        {
            player.ActivateRoad();
            Container container = collision.gameObject.GetComponentInChildren<Container>();
            container.LoseBricks();
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<Player>(out Player player))
        {
            player.DeactivateRoad();
            Container container = collision.gameObject.GetComponentInChildren<Container>();
            container.StopLoseBricks();
        }
    }
}
