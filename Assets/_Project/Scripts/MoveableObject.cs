using UnityEngine;

public abstract class MoveableObject : MonoBehaviour
{
    public delegate void OnObjectHeldDropped(MoveableObject moveableObject);
    public event OnObjectHeldDropped OnObjectPickedUp;
    public event OnObjectHeldDropped OnObjectDropped;


    public virtual MoveableObject GrabObject() {
        OnObjectPickedUp?.Invoke(this);
        return this;
    }

    public virtual void DragObject(Vector3 newPosition) {
        transform.position = newPosition;
    }

    public virtual void ReleaseObject() {
        OnObjectDropped?.Invoke(this);
    }
}
