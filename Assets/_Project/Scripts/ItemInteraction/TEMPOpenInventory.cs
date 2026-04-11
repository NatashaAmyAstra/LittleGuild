using UnityEngine;

public class TEMPOpenInventory : MonoBehaviour
{
    [SerializeField] private Inventory _inventory;

    private void OnMouseDown() {
        InventoryWindow.main.Toggle(_inventory);
    }
}
