using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using XNode;

[NodeWidth(300)]
public abstract class StoryElement : Node, IComparable
{
    public abstract string ElementName { get; }
    public abstract Color32 DisplayColor { get; }
    public abstract StoryElementTypes Type { get; }

    [HideInInspector]
    public bool Active = false;
    [HideInInspector]
    public StoryElement Next;

    [HideInInspector]
    [Input(ShowBackingValue.Never, ConnectionType.Multiple)] public NodePort PreviousNode;
    [HideInInspector]
    [Output(ShowBackingValue.Never, ConnectionType.Override)] public NodePort NextNode;

    public override object GetValue(NodePort port)
    {
        if(port.IsConnected)
            return port.Connection.node;
        return null;
    }

    public virtual void OnCreate() { }
    public virtual void OnDelete() { }

    public abstract IEnumerator Execute(GameManager managerCallback, UVNFCanvas canvas);

    public abstract void DisplayLayout(Rect layoutRect);

    public int CompareTo(object obj)
    {
        if (obj == null) return 1;
        if (!(obj is StoryElement)) return 1;
        return string.Compare(ElementName, ((StoryElement)obj).ElementName);
    }
}
