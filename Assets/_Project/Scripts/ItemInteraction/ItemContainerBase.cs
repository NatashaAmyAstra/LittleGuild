using UnityEngine;

public abstract class ItemContainerBase : MonoBehaviour
{
    public virtual void PlaceItem(ItemObject item) {
        item.OnObjectPickedUp += RemoveEventListener;
    }

    public abstract ItemObject TakeItem(ItemObject item);

    protected virtual void RemoveItem(ItemObject item) {
        item.OnObjectPickedUp -= RemoveEventListener;
    }

    public abstract int GetRoom();

    protected virtual void RemoveEventListener(MoveableObjectBase item) {
        if(item.GetType() != typeof(ItemObject))
            return;

        RemoveItem((ItemObject)item);
    }
}
