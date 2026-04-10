using UnityEngine;

[CreateAssetMenu(fileName = "WeaponGenerationStats", menuName = "Weapon Generation Stats")]
public class SOWeaponGeneration : SOGearBase
{
    public enum WeaponType {
        sword,
        axe,
        hammer,
        greatsword,
        bow
    }

    [Header("Weapon")]
    [SerializeField] private WeaponType _weaponType;
    [SerializeField] private int _minDamage;
    [SerializeField] private int _maxDamage;
    [SerializeField] private int _damageValueMultiplier;

    public override Item Generate() {
        base.GenerateGearStats();

        int damage = Random.Range(_minDamage, _maxDamage);

        int value = _baseValue;
        value += damage * _damageValueMultiplier;
        value += (int)_material * _materialValueMultiplier;

        WeaponItem weapon = new WeaponItem(_weaponType, _material, _sprite, damage, value);
        
        return weapon;
    }
}
