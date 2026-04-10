using System.Collections.Generic;
using UnityEngine;

public abstract class WalkNode : MonoBehaviour
{
    private static List<WalkNode> _nodes = new List<WalkNode>();
    public static WalkNode[] Nodes { get { return _nodes.ToArray(); } set { } }

    private void OnEnable() {
        _nodes.Add(this);
    }

    private void OnDisable() {
        _nodes.Remove(this);
    }
}
