using UnityEngine;

public class ArmorItem : GearItemBase
{
    private SOArmorGeneration.ArmorType _type;
    private int _resistance;

    public SOArmorGeneration.ArmorType Type { get { return _type; } set { } }
    public int Resistance { get { return _resistance; } set { } }

    public ArmorItem(SOArmorGeneration.ArmorType type, SOGearBase.GearMaterial material, Sprite sprite, int resistance, int value) : base(sprite, value, material) {
        _type = type;
        _resistance = resistance;
    }
}
