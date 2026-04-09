using UnityEngine;

public class ItemInstantiator : MonoBehaviour
{
    [SerializeField] private GameObject _itemPrefab;

    public GameObject InstantiateItem(SOItem itemStats, Vector3 position, Transform parent = null, bool playerCanDragItem = true) {
        GameObject itemObject = Instantiate(_itemPrefab, position, Quaternion.identity, parent);
        Item item = itemObject.GetComponent<Item>();
        item.Setup(itemStats, playerCanDragItem);
        return itemObject;
    }
}
