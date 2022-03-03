using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class MultiplierShower : MonoBehaviour
{
    [SerializeField] private MultiplierZone _multiplierZone;

    private TMP_Text _text;

    private void Awake()
    {
        _text = GetComponent<TMP_Text>();
    }

    private void Start()
    {
        Show();
    }

    private void Show()
    {
        _text.text = "x" + _multiplierZone.Multiplier;
    }
}
