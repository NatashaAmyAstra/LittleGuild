using UnityEngine;

public abstract class SOItemGeneratorBase : ScriptableObject
{
    [Header("Base")]
    [SerializeField] protected int _baseValue;
    [SerializeField] protected Sprite _sprite;

    public abstract Item Generate();
}
