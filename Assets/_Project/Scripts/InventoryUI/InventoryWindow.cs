using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryWindow : MonoBehaviour
{
    [SerializeField] private Inventory _inventory;
    [SerializeField] private Transform _contentHolder;
    private InventorySlot[] _itemSlots;

    private void GetItemSlots() {
        // get all the item slots in children.
        List<InventorySlot> slots = new List<InventorySlot>();
        foreach(Transform slotTransform in _contentHolder)
        {
            InventorySlot itemSlot = slotTransform.GetComponent<InventorySlot>();
            slots.Add(itemSlot);
            itemSlot.SetParentWindow(this);
        }

        _itemSlots = slots.ToArray();
    }

    private void LoadInventory() {
        // sets all slots' items to render them in the correct position in the inventory
        Item[] items = _inventory.Contents;

        for(int i = 0; i < _itemSlots.Length; i++)
        {
            _itemSlots[i].SetItem(items[i]);
        }
    }

    public void ExchangeItems(InventorySlot slot1, InventorySlot slot2) {
        // swaps the items in 2 given inventory slots in the inventory object
        int index1 = Array.IndexOf(_itemSlots, slot1);
        int index2 = Array.IndexOf(_itemSlots, slot2);
        _inventory.SwapItems(index1, index2);
    }

    private void Start() {
        GetItemSlots();
        LoadInventory();

        // if the inventory changes, reload invenotry to render items in correct positions
        _inventory.OnInventoryUpdated += LoadInventory;
    }

    public void RemoveItem(Item item) {
        _inventory.TakeItem(item);
        LoadInventory();
    }
}