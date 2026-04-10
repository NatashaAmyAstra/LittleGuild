using System;
using UnityEngine;
using static SOGearBase;

[CreateAssetMenu(fileName = "PotionGenerationStats", menuName = "Potion Generation Stats")]
public class SOPotionGenerator : SOItemGeneratorBase
{
    public enum PotionEffect
    {
        healing,
        poison,
        curePoison
    }

    public enum PotionQuality
    {
        lesser = -1,
        normal,
        greater,
        grand
    }

    [Header("Potion")]
    [SerializeField] private PotionEffect _type;

    private PotionQuality _quality;
    [SerializeField] private int _qualityValueMultiplier;

    public override Item Generate() {
        PotionQuality[] potionQualities = (PotionQuality[])Enum.GetValues(typeof(PotionQuality));
        int index = UnityEngine.Random.Range(0, potionQualities.Length);
        _quality = potionQualities[index];

        int value = _baseValue;
        value += _qualityValueMultiplier * (int)_quality;

        PotionItem potion = new PotionItem(_type, _quality, _sprite, value);

        return potion;
    }
}
