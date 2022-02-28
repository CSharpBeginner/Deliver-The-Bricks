using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor3 : MonoBehaviour
{
    [SerializeField] private Place3[] _places;

    public IReadOnlyList<Place3> Places => _places;

    public bool IsEmpty()
    {
        foreach (Place3 place in _places)
        {
            if (place.Brick != null)
            {
                return false;
            }
        }

        return true;
    }
}
