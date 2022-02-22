using UnityEngine;

public class Grid : MonoBehaviour
{
    [SerializeField] private Brick _prefab;

    public Vector3 GridToWorldPosition(Vector3Int gridPosition)
    {
        float positionY = gridPosition.y * _prefab.transform.localScale.y;
        float positionZ = gridPosition.z * _prefab.transform.localScale.z;
        float positionX;

        if (gridPosition.z % 2 == 0)
        {
            positionX = gridPosition.x * _prefab.transform.localScale.x;
        }
        else
        {
            positionX = gridPosition.x * _prefab.transform.localScale.x + 0.5f * _prefab.transform.localScale.x;
        }

        return new Vector3(positionX, positionY, positionZ);
    }

    public Vector3Int WorldToGridPosition(Vector3 worldPosition)
    {
        int positionY = (int)(worldPosition.y / _prefab.transform.localScale.y);
        int positionZ = (int)(worldPosition.z / _prefab.transform.localScale.z);
        int positionX;

        if (positionZ % 2 == 0)
        {
            positionX = (int)(worldPosition.x / _prefab.transform.localScale.x);
        }
        else
        {
            positionX = (int)(worldPosition.x / _prefab.transform.localScale.x + 0.5f * _prefab.transform.localScale.x);
        }

        return new Vector3Int(positionX, positionY, positionZ);
    }
}
