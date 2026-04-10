using UnityEngine;
using static SOWeaponGeneration;

[CreateAssetMenu(fileName = "ArmorGenerationStats", menuName = "Armor Generation Stats")]
public class SOArmorGeneration : SOGearBase
{
    public enum ArmorType
    {
        chestplate,
        leggings,
        boots,
        bracers,
        helmet
    }

    [Header("Armor")]
    [SerializeField] private ArmorType _armorType;
    [SerializeField] private int _minResistance;
    [SerializeField] private int _maxResistance;
    [SerializeField] private int _resistanceValueMultiplier;

    public override Item Generate() {
        base.GenerateGearStats();

        int resistance = Random.Range(_minResistance, _maxResistance);

        int value = _baseValue;
        value += resistance * _resistanceValueMultiplier;
        value += (int)_material * _materialValueMultiplier;

        ArmorItem armor = new ArmorItem(_armorType, _material, _sprite, resistance, value);

        return armor;
    }
}