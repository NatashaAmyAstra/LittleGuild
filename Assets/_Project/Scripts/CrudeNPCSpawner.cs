using UnityEngine;

public class CrudeNPCSpawner : MonoBehaviour
{
    public static CrudeNPCSpawner main;

    [SerializeField] private GameObject _prefab;
    [SerializeField] private Vector3 _position;

    private void Awake() {
        if(main == null)
            main = this;
        else
            Destroy(this);
    }

    public void SpawnNPC(int itemCount = 0) {
        Inventory inventory = Instantiate(_prefab, _position, Quaternion.identity).GetComponent<Inventory>();

        if(itemCount < 0)
        {
            itemCount = Mathf.FloorToInt(Random.value * Mathf.Abs(itemCount));
        }

        for(int i = 0; i < itemCount; i++)
        {
            inventory.PlaceItem(GearGenerator.main.GenerateItem());
        }
    }
}
