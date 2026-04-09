using UnityEngine;

[CreateAssetMenu(fileName = "item", menuName = "Store Item", order = 0)]
public class SOItem : ScriptableObject
{
    public enum ItemType {
        weapon,
        armor,
        potion
    }

    // members
    [SerializeField] private Sprite _sprite;
    [SerializeField] private int _value;
    [SerializeField] private ItemType _type;

    // properties
    public Sprite Sprite { get { return _sprite; } }
    public int Value { get { return _value; } }
    public ItemType Type { get { return _type; } }
}
