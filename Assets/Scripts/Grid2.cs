using UnityEngine;

public class Grid2 : MonoBehaviour
{
    [SerializeField] private Brick _prefab;

    public Vector3 GridToWorldPosition(Vector3Int gridPosition)
    {
        float positionY = gridPosition.y * _prefab.transform.localScale.y;
        float positionZ;// = gridPosition.z * _prefab.transform.localScale.x;
        float positionX = gridPosition.x * _prefab.transform.localScale.z;

        if (gridPosition.x % 2 == 0)
        {
            positionZ = gridPosition.z * _prefab.transform.localScale.x;
        }
        else
        {
            positionZ = gridPosition.z * _prefab.transform.localScale.x + 0.5f * _prefab.transform.localScale.x;
        }

        return new Vector3(positionX, positionY, positionZ);
    }

    public Vector3Int WorldToGridPosition(Vector3 worldPosition)
    {
        int positionY = (int)(worldPosition.y / _prefab.transform.localScale.y);
        int positionZ;// = (int)(worldPosition.z / _prefab.transform.localScale.x);
        int positionX = (int)(worldPosition.x / _prefab.transform.localScale.z);

        if (positionX % 2 == 0)
        {
            positionZ = (int)(worldPosition.z / _prefab.transform.localScale.x);
        }
        else
        {
            positionZ = (int)(worldPosition.z / _prefab.transform.localScale.x + 0.5f * _prefab.transform.localScale.x);
        }

        return new Vector3Int(positionX, positionY, positionZ);
    }
}
