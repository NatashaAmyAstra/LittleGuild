using UnityEngine;

[CreateAssetMenu(fileName = "ArmorGenerationStats", menuName = "Armor Generation Stats")]
public class SOArmorGeneration : SOItemGenerationBase
{
    public enum ArmorType
    {
        chestplate,
        leggings,
        boots,
        bracers,
        helmet
    }

    [SerializeField] private ArmorType _armorType;

    [Header("Resistance")]
    [SerializeField] private int _minResistance;
    [SerializeField] private int _maxResistance;
    [SerializeField] private int _resistanceValueMultiplier;

    [Header("Additional")]
    [SerializeField] private string _material;
    [SerializeField] private string _effectResistance;

    public ArmorType Type { get { return _armorType; } set { } }
    public int MinResistance { get { return _minResistance; } set { } }
    public int MaxResistance { get { return _maxResistance; } set { } }
    public int ResistanceValueMultiplier { get { return _resistanceValueMultiplier; } set { } }
    public string Material { get { return _material; } set { } }
    public string EffectResistance { get { return _effectResistance; } set { } }

}