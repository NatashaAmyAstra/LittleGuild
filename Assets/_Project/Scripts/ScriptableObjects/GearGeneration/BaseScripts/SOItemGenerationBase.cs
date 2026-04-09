using UnityEngine;

public abstract class SOItemGenerationBase : ScriptableObject
{
    [Header("Base")]
    [SerializeField] protected int _baseValue;
    [SerializeField] protected Sprite _sprite;

    public int BaseValue { get { return _baseValue; } set { } }
    public Sprite Sprite { get { return _sprite; } set { } }
}
