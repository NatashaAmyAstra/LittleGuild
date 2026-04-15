using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    public InventoryWindow InventoryWindow { get {  return _inventoryWindow; } set { } } 
    public Item Item { get { return _item; } set { } }

    [SerializeField] private InventoryItem _inventoryItem;
    private InventoryWindow _inventoryWindow;
    private Item _item;

    public void SetParentWindow(InventoryWindow window) {
        _inventoryWindow = window;
    }

    public void SetItem(Item item) {
        _item = item;
        _inventoryItem.SetItem(item); // tells object what item to render
    }

    void IDropHandler.OnDrop(PointerEventData eventData) {
        // when inventoryItem is dragged onto this slot, swap its item with this slot's item in the inventory object
        InventoryItem inventoryItem = eventData.pointerDrag.GetComponent<InventoryItem>();
        if(inventoryItem == null)
            return;

        _inventoryWindow.ExchangeItems(this, inventoryItem.ItemSlot);
        
    }
}
