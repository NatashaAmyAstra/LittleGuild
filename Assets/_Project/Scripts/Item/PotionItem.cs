using UnityEngine;

public class PotionItem : Item
{
    private SOPotionGenerator.PotionEffect _effect;
    private SOPotionGenerator.PotionQuality _quality;

    public SOPotionGenerator.PotionEffect Effect { get { return _effect; } set { } }
    public SOPotionGenerator.PotionQuality Quality { get { return _quality; } set { } }

    public PotionItem(SOPotionGenerator.PotionEffect effect, SOPotionGenerator.PotionQuality quality, Sprite sprite, int value) : base(sprite, value) {
        _effect = effect;
        _quality = quality;
    }
}
