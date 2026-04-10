using UnityEngine;

public class GearGenerator : MonoBehaviour
{
    public static GearGenerator main;

    [SerializeField] private SOItemGeneratorBase[] _generators;
    private SOItemGeneratorBase _generator;

    private void Awake() {
        SetSingleton();
    }

    private void SetSingleton() {
        if(main == null)
            main = this;
        else
            Destroy(this);
    }

    public Item GenerateItem() {
        int selectedIndex = Mathf.FloorToInt(Random.value * _generators.Length);
        _generator = _generators[selectedIndex];
        Item item = _generator.Generate();
        return item;
    }
}
