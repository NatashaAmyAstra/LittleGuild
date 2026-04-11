using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Inventory))]
public class NPCBehaviour : MonoBehaviour
{
    private enum NPCState {
        entering,
        browsing,
        buying,
        selling,
        leaving
    }

    private delegate IEnumerator NPCAction();
    Dictionary<NPCState, NPCAction> NPCActions;
    private Coroutine _activeCoroutine = null;

    [SerializeField] private Inventory _inventory;
    [SerializeField] private ItemDisplay _itemDisplay;
    [SerializeField] private float _walkSpeed;
    [SerializeField] private float _idleDuration;
    private Queue<Item> _sellQueue = new Queue<Item>();

    // walk nodes
    private CounterNode _counter;
    private ExitNode _exit;
    private RoamNode _roamNode;

    private ShelfNode _targetShelf;
    private ShelfNode[] _shelves;

    // state
    [SerializeField] private NPCState _state = NPCState.entering;
    private bool _isWalking = false;
    private bool _isIdle = false;

    [Header("Placeholder variables")]
    [SerializeField] private float _itemSellChance;
    [SerializeField] private float _itemBuyChance;
    [SerializeField] private float _continueBrowsingChance;

    private bool IsAt(Vector3 destination) {
        if(destination == null)
            return true;

        return transform.position == destination;
    }

    private IEnumerator MoveTo(Vector3 destination) {
        while(IsAt(destination) == false)
        {
            Vector3 newPosition = Vector3.MoveTowards(transform.position, destination, _walkSpeed * Time.deltaTime);
            transform.position = newPosition;
            yield return new WaitForEndOfFrame();
        }
    }

    private void SetState(NPCState state, bool idle = true) {
        _state = state;
        _isIdle = idle;
    }

    private void StashObject(ItemObject item) {
        _inventory.PlaceItem(item.Info);
        _itemDisplay.TakeAndDestroyItem();
    }

    private ItemObject Unstash(Item item) {
        _inventory.TakeItem(item);
        Transform parent = _itemDisplay.transform;
        ItemObject itemObject = ItemInstantiator.main.Instantiate(item).GetComponent<ItemObject>();
        _itemDisplay.PlaceItem(itemObject);
        return itemObject;
    }

    // NPC enters shop
    // decide whether to sell or not

    // has item with value => can choose to sell

    // CHOOSING TO SELL
    // walk to counter
    // grab item out of inventory
    // idle for a bit to show it off
    // sell item
    // chance to enter browsing state


    // CHOOSING NOT TO SELL
    // will browse items and add affordable items to buy list

    // after checking all items, randomly choose to end or continue
    // continue = choose random shelf node and repeat previous step

    // ending means choosing affordable item to buy
    // none found = leave store
    // item found = walk to corresponding shelf, grab item =>
    // walk to counter, puchace item
    // chance to return to browsing

    private bool WantToSell(Item item) {
        return Random.value < _itemSellChance / 100;
    }

    private bool WantToBuy(Item item) {
        if(item.Value > _inventory.Balance)
            return false;

        return Random.value < _itemBuyChance / 100;
    }

    private bool IsWantingToKeepBrowsing() {
        return Random.value < _continueBrowsingChance / 100;
    }

    private IEnumerator Entering() {
        // enter store in a random location
        yield return MoveTo(_roamNode.Position);
        yield return new WaitForSeconds(_idleDuration);

        // check inventory for items to sell
        Item[] items = _inventory.PeekItems();
        foreach(Item item in items)
        {
            if(WantToSell(item) == false)
                continue;

            _sellQueue.Enqueue(item);
        }

        // if an item was found that NPC wants to sell, sell it
        // otherwise, start browsing the store for things to buy
        if(_sellQueue.Count > 0)
        {
            SetState(NPCState.selling);
        }
        else
        {
            SetState(NPCState.browsing);
        }

        _activeCoroutine = null;
    }

    private IEnumerator Browsing() {
        // walk to random position
        yield return MoveTo(_roamNode.Position);
        yield return new WaitForSeconds(_idleDuration);
        
        // check for items to buy
        foreach(ShelfNode shelf in _shelves)
        {
            if(shelf.Item == null)
                continue;

            Item item = shelf.Item.Info;
            if(WantToBuy(item) == false)
                continue;

            // desirable item found, enter buying phase
            _targetShelf = shelf;
            SetState(NPCState.buying);
            _activeCoroutine = null;
            yield break;
        }

        // if the NPC wants to keep browsing, the state remains the same
        // and the browse routine runs again
        _activeCoroutine = null;
        if(IsWantingToKeepBrowsing())
            yield break;

        _state = NPCState.leaving;
    }

    private IEnumerator Buying() {
        // walk to shelf and check if it is still available
        yield return MoveTo(_targetShelf.Position);
        ItemObject item = _targetShelf.Item;
        if(item == null)
        {
            // if item no longer available, return to browsing state
            yield return new WaitForSeconds(_idleDuration);
            SetState(NPCState.browsing);
            _activeCoroutine = null;
            yield break;
        }

        // grab item
        _itemDisplay.PlaceItem(_targetShelf.GrabItem());
        yield return new WaitForSeconds(_idleDuration);

        // purchase item at counter
        yield return MoveTo(_counter.Position);
        _counter.PayForItem(item.Value, _inventory);
        StashObject(item);

        // continue looking for items, or leave the store
        yield return new WaitForSeconds(_idleDuration);
        if(IsWantingToKeepBrowsing())
        {
            _state = NPCState.browsing;
        }
        else
        {
            _state = NPCState.leaving;
        }

        _activeCoroutine = null;
    }

    private IEnumerator Selling() {
        // walk to counter
        yield return MoveTo(_counter.Position);

        // sell all items in sell queue
        while(_sellQueue.Count > 0)
        {
            Item item = _sellQueue.Dequeue();
            Unstash(item);
            yield return new WaitForSeconds(_idleDuration);
            _counter.SellItem(_itemDisplay.TakeAndDestroyItem(), _inventory);
            yield return new WaitForSeconds(_idleDuration);
        }

        // look for items to buy, or leave the store
        if(IsWantingToKeepBrowsing())
        {
            _state = NPCState.browsing;
        }
        else
        {
            _state = NPCState.leaving;
        }

        _activeCoroutine = null;
    }

    private IEnumerator Leaving() {
        yield return MoveTo(_exit.Position);
    }

    private void Start() {
        NPCActions = new Dictionary<NPCState, NPCAction>() {
            { NPCState.entering, Entering },
            { NPCState.browsing, Browsing },
            { NPCState.buying, Buying },
            { NPCState.selling, Selling},
            { NPCState.leaving, Leaving }
        };

        _counter = WalkNode.GetFirst<CounterNode>();
        _exit = WalkNode.GetFirst<ExitNode>();
        _roamNode = WalkNode.GetFirst<RoamNode>();
        _shelves = WalkNode.GetAll<ShelfNode>();
    }

    private void Update() {
        if(_activeCoroutine == null)
            _activeCoroutine = StartCoroutine(NPCActions[_state].Invoke());
    }
}
