using UnityEngine;

public class ItemInstantiator : MonoBehaviour
{
    public static ItemInstantiator main;

    [SerializeField] private GameObject _itemPrefab;

    private void Awake() {
        SetSingleton();
    }

    private void SetSingleton() {
        if(main == null)
            main = this;
        else
            Destroy(this);
    }

    public GameObject Instantiate(Item itemStats, Vector3 position, Transform parent = null, bool playerCanDragItem = true) {
        GameObject itemObject = Instantiate(_itemPrefab, position, Quaternion.identity, parent);
        ItemObject item = itemObject.GetComponent<ItemObject>();
        item.Setup(itemStats, playerCanDragItem);
        return itemObject;
    }

    public GameObject Instantiate(Item itemStats) {
        return Instantiate(itemStats, Vector3.zero);
    }
}
