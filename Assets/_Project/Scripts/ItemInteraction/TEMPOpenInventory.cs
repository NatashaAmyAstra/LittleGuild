using UnityEngine;

public class TEMPOpenInventory : MonoBehaviour
{
    [SerializeField] private InventoryWindow _inventoryWindow;
    [SerializeField] private Inventory _inventory;

    private void OnMouseDown() {
        _inventoryWindow.Toggle(_inventory);
    }
}
