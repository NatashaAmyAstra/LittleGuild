using UnityEngine;

public class ShelfNode : WalkNode
{
    [SerializeField] private ItemObject _initialItem;
    [SerializeField] private ItemDisplay _itemDisplay;

    public ItemObject Item { get { return _itemDisplay.Item; } set { } }
    public int Price { get { return _itemDisplay.Item.Value; } set { } }

    private void Start() {
        PlaceInitialItem();
    }

    private void PlaceInitialItem() {
        if(_initialItem == null)
            return;
        _itemDisplay.PlaceItem(_initialItem);
    }

    public ItemObject GrabItem() {
        return _itemDisplay.TakeItem();
    }
}
