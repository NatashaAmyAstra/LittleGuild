using UnityEngine;

public abstract class Item
{
    protected Sprite _sprite;
    protected int _value;

    public Sprite Sprite { get { return _sprite; } set { } }
    public int Value { get { return _value; } set { } }

    public Item(Sprite sprite, int value) {
        _sprite = sprite;
        _value = value;
    }
}
