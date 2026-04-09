using UnityEngine;

[CreateAssetMenu(fileName = "WeaponGenerationStats", menuName = "Weapon Generation Stats")]
public class SOWeaponGeneration : SOItemGenerationBase
{
    public enum WeaponType {
        sword,
        axe,
        hammer,
        greatsword,
        bow
    }

    [SerializeField] private WeaponType _weaponType;

    [Header("Damage")]
    [SerializeField] private int _minDamage;
    [SerializeField] private int _maxDamage;
    [SerializeField] private int _damageValueMultiplier;

    [Header("Additional")]
    [SerializeField] private string _material;
    [SerializeField] private string _effect;

    public WeaponType Type { get { return _weaponType; } set { } }
    public int MinDamage { get { return _minDamage; } set { } }
    public int MaxDamage { get { return _maxDamage; } set { } }
    public int DamageValueMultiplier { get { return _damageValueMultiplier; } set { } }
    public string Material { get { return _material; } set { } }
    public string Effect { get { return _effect; } set { } }

}
