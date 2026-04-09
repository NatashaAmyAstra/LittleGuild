using UnityEngine;

[CreateAssetMenu(fileName = "PotionGenerationStats", menuName = "Potion Generation Stats")]
public class SOPotionGenerator : SOItemGenerationBase
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

    [SerializeField] private PotionEffect _type;

    [Header("Quality")]
    [SerializeField] private PotionQuality _quality;
    [SerializeField] private int _qualityValueMultiplier;

    public PotionEffect Type { get { return _type; } set { } }
    public PotionQuality Quality { get { return _quality; } set { } }
    public int QualityValueMultiplier { get { return _qualityValueMultiplier; } set { } }
}
