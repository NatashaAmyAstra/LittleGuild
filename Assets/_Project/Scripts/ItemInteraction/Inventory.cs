using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public event Action OnInventoryUpdated;

    [SerializeField] private int _gold;
    [SerializeField] private bool _playerCanDragItem = false;
    [SerializeField] private int _space = 20;
    private Item[] _contents;

    #region Properties
    public int Balance { get { return _gold; } set { } }
    public bool PlayerCanDragItem { get { return _playerCanDragItem; } set { } }
    public int GetItemCount { get { return _contents.Length - GetFreeSpace; } set { } }
    public int GetFreeSpace { get {
            int count = 0;
            foreach(Item item in _contents)
                if(item == null)
                    count++;
            return count;
        } set { } }
    #endregion

    private void Awake() {
        _contents = new Item[_space];
    }

    private int GetIndex(Item item) {
        return Array.IndexOf(_contents, item);
    }

    #region Gold handling
    // handle exchanging of gold
    public bool Pay(int payment) {
        if(_gold < payment)
            return false;

        _gold -= payment;
        return true;
    }

    public bool PayToInventory(int payment, Inventory recipient) {
        if(Pay(payment) == false)
            return false;

        recipient.ReceivePayment(payment);
        return true;
    }

    public void ReceivePayment(int payment) {
        _gold += payment;
    }

    public bool ReceivePaymentFromInventory(int payment, Inventory payee) {
        if(payee.Balance < payment)
            return false;

        ReceivePayment(payment);
        payee.Pay(payment);
        return true;
    }
    #endregion

    #region Peek items
    // return item without removing it from the inventory
    public Item PeekItem(int index = 0) {
        return _contents[index];
    }

    public Item[] PeekItems() {
        List<Item> items = new List<Item>();
        foreach(Item item in _contents)
        {
            if(item == null)
                continue;
            items.Add(item);
        }

        return items.ToArray();
    }
    #endregion

    #region Add to inventory
    // Adding item(s) to the inventory
    public bool PlaceItem(Item item) {
        if(item == null)
            return false;

        if(GetIndex(item) >= 0)
            return false;

        if(GetFreeSpace <= 0)
            return false;

        for(int i = 0; i < _contents.Length; i++)
        {
            if(_contents[i] != null)
                continue;

            _contents[i] = item;
            OnInventoryUpdated?.Invoke();
            return true;
        }

        return false;
    }

    public bool PlaceItems(Item[] items) {
        if(items == null)
            return false;

        if(items.Length > GetFreeSpace)
            return false;

        foreach (Item item in items)
        {
            PlaceItem(item);
        }

        return true;
    }
    #endregion

    #region Remove from inventory
    // removes item from inventory
    private void RemoveItem(Item item) {
        _contents[GetIndex(item)] = null;
        OnInventoryUpdated?.Invoke();
    }


    // removes and returns specific item
    // selected by different methods:
    // specific item,
    // item at specific index,
    // first X amount of items in the inventory
    public Item TakeItem(Item item) {
        if(item == null)
            return null;

        int index = GetIndex(item);

        if(index < 0)
        {
            return null;
        }

        Item result = _contents[index];
        RemoveItem(_contents[index]);

        return result;
    }

    public Item TakeItemByIndex(int index) {
        if(index >= _contents.Length)
            return null;

        return TakeItem(_contents[index]);
    }

    public Item[] TakeItems(Item[] items) {
        List<Item> result = new List<Item>();
        foreach (Item item in items)
        {
            result.Add(TakeItem(item));
        }

        return result.ToArray();
    }

    public Item[] TakeItemsByCount(int count) {
        List<Item> result = new List<Item>();
        for(int i = 0; i < count; i++)
        {
            result.Add(TakeItemByIndex(_contents.Length - 1));
        }

        return result.ToArray();
    }

    public Item[] TakeItemsByIndex(int[] indices) {
        List<Item> result = new List<Item>();
        foreach(int i in indices)
        {
            if(i >= _contents.Length)
                continue;

            result.Add(_contents[i]);
        }

        foreach(Item item in result)
        {
            RemoveItem(item);
        }

        return result.ToArray();
    }
    #endregion

    #region Exchange between inventories
    // exchange items directly between two known inventories
    public void GiveItemToInventory(Item item, Inventory recipient) {
        recipient.PlaceItem(TakeItem(item));
    }

    public void ReceiveItemFromInventory(Item item, Inventory recipient) {
        PlaceItem(recipient.TakeItem(item));
    }
    #endregion
}
