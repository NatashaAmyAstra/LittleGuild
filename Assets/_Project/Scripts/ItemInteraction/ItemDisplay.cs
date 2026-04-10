using UnityEngine;

public class ItemDisplay : ItemContainerBase
{
    private ItemObject _heldItem = null;
    public ItemObject Item { get { return _heldItem; } set { } }

    public override void PlaceItem(ItemObject item) {
        base.PlaceItem(item);

        _heldItem = item;
        item.transform.position = transform.position;
        item.transform.parent = transform;
    }

    public override ItemObject TakeItem(ItemObject item) {
        if(item != _heldItem)
            return null;

        RemoveItem(item);
        return item;
    }

    public ItemObject TakeItem() {
        return TakeItem(_heldItem);
    }

    public override int GetRoom() {
        return _heldItem == null? 1 : 0;
    }

    protected override void RemoveItem(ItemObject item) {
        base.RemoveItem(item);
        _heldItem = null;
    }
}
