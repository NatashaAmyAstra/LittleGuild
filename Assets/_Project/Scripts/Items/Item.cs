using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(CircleCollider2D))]
public class Item : MoveableObject
{
    // stats come from scriptable object
    [SerializeField] private SOItem _itemSO;
    [SerializeField] private CircleCollider2D _circleCollider;
    [SerializeField] private float _snapRadius;
    [SerializeField] private LayerMask _layerMask = 6;
    [SerializeField] private bool _playerCanDragItem = true;

    // stat properties
    public SOItem ScriptableObject { get { return _itemSO; } set { } }

    public Sprite Sprite { get { return _itemSO.Sprite; } }
    public int Value { get { return _itemSO.Value; } }
    public SOItem.ItemType Type { get { return _itemSO.Type; } set { } }

    [SerializeField] private SpriteRenderer _itemRenderer;

    private void Awake() {
        Setup(_itemSO);
    }

    public void Setup(SOItem item, bool playerCanDragItem = true) {
        _itemSO = item;
        _itemRenderer.sprite = _itemSO.Sprite;

        _playerCanDragItem = playerCanDragItem;
    }


    public void DestroyItem() {
        Destroy(gameObject);
    }

    public override MoveableObject GrabObject() {
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
        ItemContainer container = null;
        for(int i = 0; i < containerColliders.Length; i++)
        {
            ItemContainer testContainer = containerColliders[i].GetComponent<ItemContainer>();

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
