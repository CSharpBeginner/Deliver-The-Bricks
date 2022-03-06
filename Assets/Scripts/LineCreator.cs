using UnityEngine;

public class LineCreator : MonoBehaviour
{
    [SerializeField] private Brick _prefab;
    [SerializeField] private float _offset;
    [SerializeField] private int _halfOfCount;

    [ContextMenu("Create")]
    private void Create()
    {
        Quaternion rightRotation = Quaternion.Euler(0, 90, 0);
        Vector3 translation;
        Vector3 startPosition;
        Vector3 translation1;
        Vector3 translation2;

        float startXposition = (_prefab.transform.localScale.z + _offset) / 2 + (_halfOfCount - 1) * (_prefab.transform.localScale.z + _offset);
        startPosition = new Vector3(startXposition, 0, 0);

        if (_halfOfCount % 2 == 0)
        {
            translation1 = new Vector3(_prefab.transform.localScale.z + _offset, 0, _prefab.transform.localScale.x / 2);
            translation2 = new Vector3(_prefab.transform.localScale.z + _offset, 0, 0);
        }
        else
        {
            translation1 = new Vector3(_prefab.transform.localScale.z + _offset, 0, 0);
            translation2 = new Vector3(_prefab.transform.localScale.z + _offset, 0, _prefab.transform.localScale.x / 2);
        }

        for (int i = 0; i < _halfOfCount * 2; i++)
        {
            if (i % 2 == 1)
            {
                translation = translation1;
            }
            else
            {
                translation = translation2;
            }

            Vector3 position = startPosition + new Vector3(translation.x * i, translation.y, translation.z);
            GameObject gameObject = new GameObject($"Place {i}", typeof(Place));
            gameObject.transform.parent = transform;
            gameObject.transform.localPosition = position * transform.localScale.x;
            gameObject.transform.rotation = rightRotation;
        }
    }
}
