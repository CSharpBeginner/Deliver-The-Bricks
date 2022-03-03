using UnityEngine;
using UnityEngine.Events;

public class Inputer : MonoBehaviour
{
    private Vector2 _startPosition;

    public event UnityAction TouchStarted;
    public event UnityAction<float> NormalizedDifferenceChanged;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _startPosition = Input.mousePosition;
            TouchStarted?.Invoke();
        }

        if (Input.GetMouseButton(0))
        {
            Vector2 mousePosition = Input.mousePosition;
            float normalizedDifference = (mousePosition.x - _startPosition.x) / Screen.width;
            NormalizedDifferenceChanged?.Invoke(normalizedDifference);
        }
    }
}
