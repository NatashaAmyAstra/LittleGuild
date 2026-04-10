using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public event Action OnInventoryUpdated;

    [SerializeField] private int _gold;
    [SerializeField] private bool _playerCanDragItem = false;
    [SerializeField] private int _space = 20;
    private List<Item> _contents = new List<Item>();

    public int Balance { get { return _gold; } set { } }
    public bool PlayerCanDragItem { get { return _playerCanDragItem; } set { } }
    public int GetCount { get { return _contents.Count; } set { } }
    public int GetFreeSpace { get { return _space - _contents.Count; } set { } }


    // handle exchanging of gold
    public void PayUp(int payment) {
        _gold -= payment;
    }

    public void PayToInventory(int payment, Inventory recipient) {
        PayUp(payment);
        recipient.ReceivePayment(payment);
    }

    public void ReceivePayment(int payment) {
        _gold += payment;
    }

    public void ReceivePaymentFromInventory(int payment, Inventory payee) {
        ReceivePayment(payment);
        payee.PayUp(payment);
    }


    // return item without removing it from the inventory
    public Item PeekItem(int index = 0) {
        return _contents[index];
    }

    public Item[] PeekItems() {
        return _contents.ToArray();
    }


    // handle adding and removing items from inventory
    public void PlaceItem(Item item) {
        if(item == null)
            return;

        if(_contents.IndexOf(item) >= 0)
            return;

        _contents.Add(item);
        OnInventoryUpdated?.Invoke();
    }

    public void PlaceItems(Item[] items) {
        if(items == null)
            return;

        foreach (Item item in items)
        {
            PlaceItem(item);
        }
    }

    private void RemoveItem(Item item) {
        _contents.RemoveAt(_contents.IndexOf(item));
        OnInventoryUpdated?.Invoke();
    }

    public Item TakeItem(Item item) {
        if(item == null)
            return null;

        int index = _contents.IndexOf(item);

        if(index < 0)
        {
            return null;
        }

        Item result = _contents[index];
        RemoveItem(_contents[index]);

        return result;
    }

    public Item TakeItemByIndex(int index) {
        if(index >= _contents.Count)
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
            result.Add(TakeItemByIndex(_contents.Count - 1));
        }

        return result.ToArray();
    }

    public Item[] TakeItemsByIndex(int[] indices) {
        List<Item> result = new List<Item>();
        foreach(int i in indices)
        {
            if(i >= _contents.Count)
                continue;

            result.Add(_contents[i]);
        }

        foreach(Item item in result)
        {
            RemoveItem(item);
        }

        return result.ToArray();
    }

    public void GiveItemToInventory(Item item, Inventory recipient) {
        recipient.PlaceItem(TakeItem(item));
    }

    public void ReceiveItemFromInventory(Item item, Inventory recipient) {
        PlaceItem(recipient.TakeItem(item));
    }
}
