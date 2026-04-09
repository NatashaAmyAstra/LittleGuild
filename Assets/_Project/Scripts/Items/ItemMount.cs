using UnityEngine;

public class ItemMount : ItemContainer
{
    private Item _heldItem = null;
    public Item Item { get { return _heldItem; } set { } }

    public override void PlaceItem(Item item) {
        base.PlaceItem(item);

        _heldItem = item;
        item.transform.position = transform.position;
        item.transform.parent = transform;
    }

    public override Item TakeItem(Item item) {
        RemoveItem(item);
        return item;
    }

    public Item TakeItem() {
        return TakeItem(_heldItem);
    }

    public override bool HasRoom() {
        return _heldItem == null;
    }

    protected override void RemoveItem(Item item) {
        base.RemoveItem(item);
        _heldItem = null;
    }
}
