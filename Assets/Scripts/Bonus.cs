using UnityEngine;
using TMPro;

public class Bonus : MonoBehaviour
{
    [SerializeField] private int _count;
    private TMP_Text _text;

    private void Awake()
    {
        _text = GetComponentInChildren<TMP_Text>();
        _text.text = _count.ToString();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Player>(out Player player))
        {
            Container container = other.GetComponentInChildren<Container>();
            container.Fill(_count);
            Destroy(gameObject);
        }
    }
}
