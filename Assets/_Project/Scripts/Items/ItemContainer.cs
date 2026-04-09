using UnityEngine;

public abstract class ItemContainer : MonoBehaviour
{
    public virtual void PlaceItem(Item item) {
        item.OnObjectPickedUp += RemoveEventListener;
    }

    public abstract Item TakeItem(Item item);

    protected virtual void RemoveItem(Item item) {
        item.OnObjectPickedUp -= RemoveEventListener;
    }

    public abstract bool HasRoom();

    protected virtual void RemoveEventListener(MoveableObject item) {
        if(item.GetType() != typeof(Item))
            return;

        RemoveItem((Item)item);
    }
}
