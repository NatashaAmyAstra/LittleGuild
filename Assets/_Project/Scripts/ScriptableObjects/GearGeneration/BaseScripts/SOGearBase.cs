using System;
using UnityEngine;

public abstract class SOGearBase : SOItemGeneratorBase
{
    public enum GearMaterial {
        copper = -1,
        iron,
        mithril,
        adamant
    }

    [Header("Material")]
    protected GearMaterial _material;
    [SerializeField] protected int _materialValueMultiplier;
    [SerializeField] protected string _effect;

    protected void GenerateGearStats() {
        GearMaterial[] gearMaterials = (GearMaterial[])Enum.GetValues(typeof(GearMaterial));
        int index = UnityEngine.Random.Range(0, gearMaterials.Length);
        _material = gearMaterials[index];
    }
}
