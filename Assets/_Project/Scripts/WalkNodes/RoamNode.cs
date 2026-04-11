using UnityEngine;

public class RoamNode : WalkNode
{
    public override Vector3 Position { get { return GetRandomPosition(); } set { } }

    [SerializeField] private Transform _leftBound;
    [SerializeField] private Transform _rightBound;
    private float _boundDistance;

    private void Awake() {
        _boundDistance = _rightBound.position.x - _leftBound.position.x;
    }

    private Vector3 GetRandomPosition() {
        Vector3 position = _leftBound.position;
        position.x += Random.value * _boundDistance;
        return position;
    }
}

public class RoamPosition
{
    private RoamNode _node;
    public RoamNode Node { get { return _node; } set { } }

    private Vector3 _position;
    public Vector3 Position { get { return _position; } set { } }

    public RoamPosition(RoamNode node, Vector3 position) {
        _node = node;
        _position = position;
    }
}
