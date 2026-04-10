using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InventoryWindow : ItemContainerBase
{
    [SerializeField] private GameObject _inventoryWindow;
    [SerializeField] private Collider2D _inventoryCollider;
    [SerializeField] private TextMeshPro _balanceText;
    [SerializeField] private ItemInstantiator _itemInstantiator;
    [SerializeField] private Vector3 _inventoryItemBounds;
    private Vector3 _inventoryCenterPoint;
    
    private Inventory _selectedInventory;
    private List<ItemObject> _displayedItems = new List<ItemObject>();


    public override void PlaceItem(ItemObject item) {
        base.PlaceItem(item);
        _displayedItems.Add(item);
        _selectedInventory.PlaceItem(item.ScriptableObject);
    }

    public override ItemObject TakeItem(ItemObject item) {
        RemoveItem(item);
        return item;
    }

    protected override void RemoveItem(ItemObject item) {
        base.RemoveItem(item);
        _displayedItems.Remove(item);
        _selectedInventory.TakeItem(item.ScriptableObject);
    }

    public override bool HasRoom() {
        return _selectedInventory.GetFreeSpace > 0;
    }


    private void Awake() {
        _inventoryCenterPoint = _inventoryWindow.transform.position;
    }

    public void ToggleWindow(Inventory inventory) {
        if(_selectedInventory == inventory || inventory == null)
        {
            CloseWindow();
            return;
        }

        if(_selectedInventory != null)
        {
            CloseWindow();
        }

        OpenWindow(inventory);
    }

    public void OpenWindow(Inventory inventory) {
        _selectedInventory = inventory;
        _selectedInventory.OnInventoryUpdated += UpdateWindow;
        UpdateWindow();
    }

    public void CloseWindow() {
        _selectedInventory.OnInventoryUpdated -= UpdateWindow;
        _selectedInventory = null;
        UpdateWindow();
    }

    private void UpdateWindow() {
        if (_selectedInventory == null)
        {
            DeactivateWindow();
        }
        else
        {
            ActivateWindow();
        }
    }

    private void DeactivateWindow() {
        _inventoryWindow.SetActive(false);
        _inventoryCollider.enabled = false;

        // destroy each item within the inventory
        RemoveItems();
    }

    private void ActivateWindow() {
        RemoveItems(); // remove items from previous inventory
        _inventoryWindow.SetActive(true);
        _inventoryCollider.enabled = true;

        // write balance
        _balanceText.text = _selectedInventory.Balance.ToString();

        // spawn each item in the inventory
        SpawnItems();
    }

    private void RemoveItems() {
        foreach(ItemObject item in _displayedItems)
        {
            Destroy(item.gameObject);
        }
        _displayedItems.Clear();
    }

    private void SpawnItems() {
        Item[] items = _selectedInventory.PeekItems();
        foreach(Item item in items)
        {
            Vector3 randomPos = _inventoryItemBounds;
            randomPos.x *= (Random.value - 0.5f) * 2;
            randomPos.y *= (Random.value - 0.5f) * 2;
            randomPos += _inventoryCenterPoint;

            bool canDrag = _selectedInventory.PlayerCanDragItem;

            GameObject spawnedObject = _itemInstantiator.InstantiateItem(item, randomPos, transform, canDrag);
            PlaceItem(spawnedObject.GetComponent<ItemObject>());
        }
    }
}
