using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public event Action OnInventoryUpdated;

    [SerializeField] private int _gold;
    [SerializeField] private bool _playerCanDragItem = false;
    [SerializeField] private List<SOItem> _contents = new List<SOItem>();
    [SerializeField] private int _space = 20;

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
    public SOItem PeekItem(int index = 0) {
        return _contents[index];
    }

    public SOItem[] PeekItems() {
        return _contents.ToArray();
    }


    // handle adding and removing items from inventory
    public void PlaceItem(SOItem item) {
        if(item == null)
            return;

        if(_contents.IndexOf(item) >= 0)
            return;

        _contents.Add(item);
        OnInventoryUpdated?.Invoke();
    }

    public void PlaceItems(SOItem[] items) {
        if(items == null)
            return;

        foreach (SOItem item in items)
        {
            PlaceItem(item);
        }
    }


    private void RemoveItem(SOItem item) {
        _contents.RemoveAt(_contents.IndexOf(item));
        OnInventoryUpdated?.Invoke();
    }

    public SOItem TakeItem(SOItem item) {
        if(item == null)
            return null;

        int index = _contents.IndexOf(item);

        if(index < 0)
        {
            return null;
        }

        SOItem result = _contents[index];
        RemoveItem(_contents[index]);

        return result;
    }

    public SOItem TakeItemByIndex(int index) {
        if(index >= _contents.Count)
            return null;

        return TakeItem(_contents[index]);
    }

    public SOItem[] TakeItems(SOItem[] items) {
        List<SOItem> result = new List<SOItem>();
        foreach (SOItem item in items)
        {
            result.Add(TakeItem(item));
        }

        return result.ToArray();
    }

    public SOItem[] TakeItemsByCount(int count) {
        List<SOItem> result = new List<SOItem>();
        for(int i = 0; i < count; i++)
        {
            result.Add(TakeItemByIndex(_contents.Count - 1));
        }

        return result.ToArray();
    }

    public SOItem[] TakeItemsByIndex(int[] indices) {
        List<SOItem> result = new List<SOItem>();
        foreach(int i in indices)
        {
            if(i >= _contents.Count)
                continue;

            result.Add(_contents[i]);
        }

        foreach(SOItem item in result)
        {
            RemoveItem(item);
        }

        return result.ToArray();
    }

    public void GiveItemToInventory(SOItem item, Inventory recipient) {
        recipient.PlaceItem(TakeItem(item));
    }

    public void ReceiveItemFromInventory(SOItem item, Inventory recipient) {
        PlaceItem(recipient.TakeItem(item));
    }
}
