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
    public Item ScriptableObject { get { return _item; } set { } }
    public Sprite Sprite { get { return _item.Sprite; } }
    public int Value { get { return _item.Value; } }
    public System.Type Type { get { return _item.GetType(); } set { } }

    [SerializeField] private SpriteRenderer _itemRenderer;

    private void Start() {
        if(_item != null)
            return;

        Setup(GearGenerator.main.GenerateItem());
    }

    public void Setup(Item item, bool playerCanDragItem = true) {
        _item = item;
        _itemRenderer.sprite = _item.Sprite;

        _playerCanDragItem = playerCanDragItem;
    }


    public void DestroyItem() {
        Destroy(gameObject);
    }

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




    private void PlaceInClosestContainer() {
        // check if there's a mount nearby. Only proceed if at least one is found
        Collider2D[] containerColliders = Physics2D.OverlapCircleAll(transform.position, _snapRadius, _layerMask);
        if(containerColliders.Length == 0)
            return;

        // select the closest mount
        ItemContainerBase container = null;
        for(int i = 0; i < containerColliders.Length; i++)
        {
            ItemContainerBase testContainer = containerColliders[i].GetComponent<ItemContainerBase>();

            if(container != null)
            {
                float closestContainerDistance = Vector3.Distance(transform.position, container.transform.position);
                float containerDistance = Vector3.Distance(transform.position, testContainer.transform.position);
                if(containerDistance > closestContainerDistance)
                    continue;
            }

            if(testContainer != null && testContainer.HasRoom())
            {
                container = testContainer;
            }
        }

        // place the item on the mount
        if(container != null)
        {
            container.PlaceItem(this);
        }
    }
}
