using System;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(CircleCollider2D))]
public class ItemObject : MoveableObjectBase
{
    // stats come from scriptable object
    private Item _item;
    [SerializeField] private CircleCollider2D _circleCollider;
    [SerializeField] private float _snapRadius;
    [SerializeField] private LayerMask _layerMask = 6;
    [SerializeField] private bool _playerCanDragItem = true;

    // stat properties
    public Item Info { get { return _item; } set { } }
    public Sprite Sprite { get { return _item.Sprite; } }
    public int Value { get { return _item.Value; } }
    public Type Type { get { return _item.GetType(); } set { } }

    [SerializeField] private SpriteRenderer _itemRenderer;

    private void Start() {
        // if object is placed without setup instructions, generate a random item
        if(_item != null)
            return;

        Setup(GearGenerator.main.GenerateItem());
    }

    public void Setup(Item item, bool playerCanDragItem = true) {
        // set identifying values
        _item = item;

        _itemRenderer.sprite = _item.Sprite;
        _playerCanDragItem = playerCanDragItem;
    }


    public void DestroyItem() {
        Destroy(gameObject);
    }

    #region Inherited methods
    public override MoveableObjectBase GrabObject() {
        if(_playerCanDragItem == false)
            return null;

        transform.parent = null;
        return base.GrabObject();
    }

    public override void ReleaseObject() {
        PlaceInClosestContainer();
        base.ReleaseObject();
    }
    #endregion


    private void PlaceInClosestContainer() {
        // check if there's a container nearby. Only proceed if at least one is found
        Collider2D[] containerColliders = Physics2D.OverlapCircleAll(transform.position, _snapRadius, _layerMask);
        if(containerColliders.Length == 0)
            return;

        // select the closest container
        ItemContainerBase closestContainer = null;
        for(int i = 0; i < containerColliders.Length; i++)
        {
            ItemContainerBase container = containerColliders[i].GetComponent<ItemContainerBase>();

            if(closestContainer != null)
            {
                // ignore container if it's further than currently closest container
                float closestContainerDistance = Vector3.Distance(transform.position, closestContainer.transform.position);
                float containerDistance = Vector3.Distance(transform.position, container.transform.position);
                if(containerDistance > closestContainerDistance)
                    continue;
            }

            // if container has room, set closest container
            if(container != null && container.GetRoom() > 0)
            {
                closestContainer = container;
            }
        }

        // place the item in the container
        if(closestContainer != null)
        {
            closestContainer.PlaceItem(this);
        }
    }
}
