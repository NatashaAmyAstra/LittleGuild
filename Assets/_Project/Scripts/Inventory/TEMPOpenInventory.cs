using UnityEngine;

public class TEMPOpenInventory : MonoBehaviour
{
    [SerializeField] private Inventory _inventory;

    private void OnMouseDown() {
        OLDInventoryWindow.main.Toggle(_inventory);
    }
}
