using System.Collections.Generic;
using UnityEngine;

public abstract class WalkNode : MonoBehaviour
{
    private static List<WalkNode> _nodes = new List<WalkNode>();
    public static WalkNode[] Nodes { get { return _nodes.ToArray(); } set { } }

    public virtual Vector3 Position { get { return transform.position; } set { } }

    public static T GetFirst<T>() where T : WalkNode {
        foreach(WalkNode node in _nodes)
        {
            if(node.GetType() == typeof(T))
                return (T)node;
        }
        return null;
    }

    public static T[] GetAll<T>() where T : WalkNode {
        List<T> nodes = new List<T>();
        foreach(WalkNode node in _nodes)
        {
            if(node.GetType() == typeof(T))
                nodes.Add((T)node);
        }
        return nodes.ToArray();
    }

    private void OnEnable() {
        _nodes.Add(this);
    }

    private void OnDisable() {
        _nodes.Remove(this);
    }
}
