using JetBrains.Annotations;
using System.Collections;
using UnityEngine;

public class CrudeNPCSpawner : MonoBehaviour
{
    public static CrudeNPCSpawner main;

    [SerializeField] private GameObject _prefab;

    [SerializeField] private int _minSpawnDelay;
    [SerializeField] private int _maxSpawnDelay;
    [SerializeField] private int _NPCItemCount;
    [SerializeField] private int _NPCGoldMin;
    [SerializeField] private int _NPCGoldMax;

    private void Awake() {
        if(main == null)
            main = this;
        else
            Destroy(this);
    }

    private void Start() {
        StartCoroutine(SpawnLoop());
    }

    private IEnumerator SpawnLoop() {
        while(true)
        {
            SpawnNPC(_NPCItemCount);
            int waitTime = _minSpawnDelay + Mathf.FloorToInt(Random.value * (_maxSpawnDelay - _minSpawnDelay));
            yield return new WaitForSeconds(waitTime);
        }
    }

    public void SpawnNPC(int itemCount = 0) {
        Inventory inventory = Instantiate(_prefab, transform.position, Quaternion.identity).GetComponent<Inventory>();

        // if item count is negative, choose a random number of items up to Abs(itemCount)
        if(itemCount < 0)
        {
            itemCount = Mathf.FloorToInt(Random.value * Mathf.Abs(itemCount));
        }

        // generate items and give them to NPC
        for(int i = 0; i < itemCount; i++)
        {
            inventory.PlaceItem(GearGenerator.main.GenerateItem());
        }

        // give NPC starting cash
        inventory.ReceivePayment(_NPCGoldMin + Mathf.FloorToInt(Random.value * (_NPCGoldMax - _NPCGoldMin)));
    }
}
