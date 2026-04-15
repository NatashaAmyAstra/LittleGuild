using UnityEngine;

public class MoveableObjectBase : MonoBehaviour
{
    public delegate void OnObjectHeldDropped(MoveableObjectBase moveableObject);
    public event OnObjectHeldDropped OnObjectPickedUp;
    public event OnObjectHeldDropped OnObjectDropped;


    public virtual MoveableObjectBase GrabObject() {
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
