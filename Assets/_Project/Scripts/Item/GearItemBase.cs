using UnityEngine;

public abstract class GearItemBase : Item
{
    private SOGearBase.GearMaterial _material;

    public SOGearBase.GearMaterial Material { get { return _material; } set { } }

    public GearItemBase (Sprite sprite, int value, SOGearBase.GearMaterial material) : base(sprite, value) {
        _material = material;
    }
}
