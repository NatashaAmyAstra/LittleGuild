using UnityEngine;

public class WeaponItem : GearItemBase
{
    private SOWeaponGeneration.WeaponType _type;
    private int _damage;

    public SOWeaponGeneration.WeaponType Type { get { return _type; } set { } }
    public int Damage { get { return _damage; } set { } }

    public WeaponItem(SOWeaponGeneration.WeaponType type, SOGearBase.GearMaterial material, Sprite sprite, int damage, int value) : base(sprite, value, material) {
        _type = type;
        _damage = damage;
    }
}
