using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public InventorySlot ItemSlot { get { return _inventorySlot; } set { } }

    [SerializeField] private InventorySlot _inventorySlot;
    [SerializeField] private Image _imageRenderer;
    [SerializeField] private int _playAreaYPosition;

    private Vector3 _clickOffset;
    private Transform _parentAfterDrag;
    private Item _item;

    public void SetItem(Item item) {
        if(item == null)
        {
            _imageRenderer.enabled = false;
            return;
        }

        _item = item;
        _imageRenderer.sprite = _item.Sprite;
        _imageRenderer.enabled = true;
    }

    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData) {
        if(_item == null)
            return;

        _imageRenderer.raycastTarget = false; // disable raycast target to read which slot this item is dropped on
        _parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
        _clickOffset = transform.position - (Vector3)eventData.position;
    }

    void IDragHandler.OnDrag(PointerEventData eventData) {
        if(_item == null)
            return;

        transform.position = (Vector3)eventData.position + _clickOffset;
    }

    void IEndDragHandler.OnEndDrag(PointerEventData eventData) {
        if(transform.position.y > _playAreaYPosition)
        {
            SpawnItemInWorld();
        }

        _imageRenderer.raycastTarget = true; // reenable racasting
        transform.SetParent(_parentAfterDrag);
    }

    private void SpawnItemInWorld() {
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(transform.position);
        worldPosition.z = 0;
        ItemInstantiator.main.Instantiate(_item, worldPosition);

        _inventorySlot.InventoryWindow.RemoveItem(_item);
    }
}
