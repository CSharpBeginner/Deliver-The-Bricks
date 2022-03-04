using System.Collections.Generic;
using UnityEngine;

public class Floor : MonoBehaviour
{
    [SerializeField] private Place[] _places;

    public IReadOnlyList<Place> Places => _places;

    public bool IsEmpty()
    {
        foreach (Place place in _places)
        {
            if (place.Brick != null)
            {
                return false;
            }
        }

        return true;
    }
}
