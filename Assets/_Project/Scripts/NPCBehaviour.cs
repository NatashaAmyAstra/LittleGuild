using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Inventory))]
public class NPCBehaviour : MonoBehaviour
{
    private enum NPCState {
        browsing,
        checkingOut,
        leaving,
        selling
    }

    [SerializeField] private Inventory _inventory;
    [SerializeField] private ItemDisplay _itemDisplay;
    [SerializeField] private float _walkSpeed;
    [SerializeField] private float _idleDuration;
    private float _idleTimer;

    // walk nodes
    private WalkNode _counterNode;
    private WalkNode _exitNode;

    private Stack<WalkNode> _browseNodes;
    private WalkNode _destinationNode;

    // state
    [SerializeField] private NPCState _npcState;
    private bool _isWalking = false;
    private bool _isIdle = false;


    private void Start() {
        _idleTimer = _idleDuration;
        _browseNodes = GetWalkNodes();

        // setting destination to counter if coming in to sell
        // temporary, until NPC decision making is introducsed
        if(_npcState == NPCState.selling)
        {
            _destinationNode = _counterNode;
            _isWalking = true;
        }
    }

    private Stack<WalkNode> GetWalkNodes() {
        Stack<WalkNode> nodeStack = new Stack<WalkNode>();

        // sort walk nodes by node type
        foreach(WalkNode node in WalkNode.Nodes)
        {
            Type type = node.GetType();
            if(type == typeof(ShelfNode))
            {
                nodeStack.Push(node);
            }
            else if(type == typeof(CounterNode))
            {
                _counterNode = node;
            }
            else if(type == typeof(ExitNode))
            {
                _exitNode = node;
            }
        }

        return nodeStack;
    }

    private void Update() {
        if(_isIdle)
        {
            Idle();
            return;
        }

        // state machine
        switch(_npcState)
        {
            case NPCState.browsing:
                Browse();
                break;
            case NPCState.checkingOut:
                CheckOut();
                break;
            case NPCState.leaving:
                Leave();
                break;
            case NPCState.selling:
                Sell();
                break;
        }
    }

    // behaviour describing the browsing state
    private void Browse() {
        // purchase the item NPC is holding
        if(_itemDisplay.Item != null)
        {
            SetState(NPCState.checkingOut, _counterNode);
            return;
        }

        if(_isWalking)
        {
            WalkToNode();
        }
        else
        {
            // if there are no browse nodes left at this stage, leave the store
            if(ChooseNextNode() == false)
            {
                SetState(NPCState.leaving, _exitNode);
                return;
            }

            // if a position is chosen, start walking to it
            _isWalking = true;
        }

        if(IsAtDestinationNode() == false)
            return;

        _isWalking = false;
        _isIdle = true;

        GrabItem();
    }

    private bool ChooseNextNode() {
        if(_browseNodes.TryPop(out _destinationNode))
        {
            _isIdle = true;
            return true;
        }
        return false;
    }

    private void GrabItem() {
        ShelfNode shelf = (ShelfNode)_destinationNode;

        if(shelf.Item == null)
            return;

        if(shelf.Price <= _inventory.Balance)
        {
            _itemDisplay.PlaceItem(shelf.GrabItem());
        }
    }


    // behaviour describing the checkingOut state
    private void CheckOut() {
        if(_isWalking)
        {
            WalkToNode();
        }

        if(IsAtDestinationNode() == false)
            return;

        // puchace item
        CounterNode counter = (CounterNode)_destinationNode;
        int price = _itemDisplay.Item.Value;
        counter.PayForItem(price, _inventory);

        // place item in inventory and remove gameobject representation of item
        _inventory.PlaceItem(_itemDisplay.Item.ScriptableObject);
        _itemDisplay.Item.DestroyItem();

        SetState(NPCState.leaving, _exitNode);
    }


    // behaviour describing the selling state
    private void Sell() {
        if(_isWalking)
        {
            WalkToNode();
        }

        if(IsAtDestinationNode() == false)
            return;

        CounterNode counter = (CounterNode)_counterNode;
        counter.SellItem(_inventory.TakeItemByIndex(0), _inventory);

        SetState(NPCState.leaving, _exitNode);
    }


    // behaviour describing the leaving state... duh
    private void Leave() {
        WalkToNode();
    }

    
    // behavioural methods

    private void SetState(NPCState state, WalkNode destination) {
        _destinationNode = destination;
        _isWalking = true;

        _npcState = state;
        _isIdle = true;
    }

    private bool IsAtDestinationNode() {
        if(_destinationNode == null) return true;
        Vector3 destination = _destinationNode.transform.position;
        return transform.position == destination;
    }

    private void WalkToNode() {
        Vector3 destination = _destinationNode.transform.position;
        Vector3 newPosition = Vector3.MoveTowards(transform.position, destination, _walkSpeed * Time.deltaTime);
        transform.position = newPosition;
    }

    private void Idle() {
        _idleTimer -= Time.deltaTime;
        if(_idleTimer <= 0)
        {
            _idleTimer = _idleDuration;
            _isIdle = false;
            return;
        }
    }
}
