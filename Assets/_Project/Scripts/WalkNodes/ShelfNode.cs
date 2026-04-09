using UnityEngine;

public class ShelfNode : WalkNode
{
    [SerializeField] private Item _initialItem;
    [SerializeField] private ItemMount _itemMount;

    public Item Item { get { return _itemMount.Item; } set { } }
    public int Price { get { return _itemMount.Item.Value; } set { } }

    private void Start() {
        PlaceInitialItem();
    }

    private void PlaceInitialItem() {
        if(_initialItem == null)
            return;
        _itemMount.PlaceItem(_initialItem);
    }

    public Item GrabItem() {
        return _itemMount.TakeItem();
    }
}
