using UnityEngine;

[RequireComponent(typeof(Inventory))]
public class CounterNode : WalkNode
{
    [SerializeField] private Inventory _inventory;

    public void PayForItem(int value, Inventory payee) {
        _inventory.ReceivePaymentFromInventory(value, payee);
    }

    public void SellItem(Item item, Inventory sellerInventory) {
        _inventory.PlaceItem(item);
        _inventory.PayToInventory(item.Value, sellerInventory);
    }
}
