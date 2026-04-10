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
    private Vector3 _centerPoint { get { return _inventoryWindow.transform.position; } set { } }
    
    private Inventory _selectedInventory;
    private List<ItemObject> _displayedItems = new List<ItemObject>();

    #region Inherited methods
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

    public override int GetRoom() {
        return _selectedInventory.GetFreeSpace;
    }
    #endregion

    #region Internal methods
    private void RemoveItems() {
        foreach(ItemObject item in _displayedItems)
        {
            Destroy(item.gameObject);
        }
        _displayedItems.Clear();
    }

    private void InstantiateItems() {
        Item[] items = _selectedInventory.PeekItems();
        foreach(Item item in items)
        {
            Vector3 randomPos = _inventoryItemBounds;
            randomPos.x *= (Random.value - 0.5f) * 2;
            randomPos.y *= (Random.value - 0.5f) * 2;
            randomPos += _centerPoint;

            bool canDrag = _selectedInventory.PlayerCanDragItem;

            GameObject spawnedObject = _itemInstantiator.InstantiateItem(item, randomPos, transform, canDrag);
            PlaceItem(spawnedObject.GetComponent<ItemObject>());
        }
    }

    private void UpdateWindow() {
        if(_selectedInventory == null)
        {
            StopRender();
        }
        else
        {
            Render();
        }
    }

    private void Render() {
        RemoveItems(); // remove items from previous inventory
        _inventoryWindow.SetActive(true);
        _inventoryCollider.enabled = true;

        // write balance
        _balanceText.text = _selectedInventory.Balance.ToString();

        // spawn each item in the inventory
        InstantiateItems();
    }

    private void StopRender() {
        _inventoryWindow.SetActive(false);
        _inventoryCollider.enabled = false;

        // destroy each item within the inventory
        RemoveItems();
    }
    #endregion

    #region Public methods
    public void Show(Inventory inventory) {
        _selectedInventory = inventory;
        _selectedInventory.OnInventoryUpdated += UpdateWindow;
        UpdateWindow();
    }

    public void Close() {
        _selectedInventory.OnInventoryUpdated -= UpdateWindow;
        _selectedInventory = null;
        UpdateWindow();
    }

    public void Toggle(Inventory inventory) {
        if(_selectedInventory == inventory || inventory == null)
        {
            Close();
            return;
        }

        if(_selectedInventory != null)
        {
            Close();
        }

        Show(inventory);
    }
    #endregion


    // TEMPORARY SHIT FOR TESTING
    public void AddRandomItemToInventory() {
        if(_selectedInventory == null)
            return;

        Item item = GearGenerator.main.GenerateItem();
        _selectedInventory.PlaceItem(item);
    }
}
