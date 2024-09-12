using UnityEngine;

public class MultiplierZone : MonoBehaviour
{
    [SerializeField] private int _multiplier;
    [SerializeField] private GameObject _confetti;

    public int Multiplier => _multiplier;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Player>() != null)
        {
            _confetti.SetActive(true);
        }
    }
}
