using UnityEngine;

[RequireComponent(typeof(Inventory))]
public class CounterNode : WalkNode
{
    [SerializeField] private Inventory _inventory;

    public void PayForItem(int value, Inventory payee) {
        _inventory.ReceivePaymentFromInventory(value, payee);
    }

    public bool SellItem(Item item, Inventory sellerInventory) {
        if(_inventory.GetFreeSpace == 0)
            return false;

        if(_inventory.PayToInventory(item.Value, sellerInventory) == false)
            return false;

        _inventory.PlaceItem(item);
        return true;
    }
}
