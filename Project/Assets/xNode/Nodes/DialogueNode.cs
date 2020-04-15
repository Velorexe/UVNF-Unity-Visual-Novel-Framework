using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

[NodeTint("#FFFFFF"), Serializable]
public class DialogueNode : Node
{
    [Input(ShowBackingValue.Never, ConnectionType.Override)] public NodePort Previous;
    [Output(ShowBackingValue.Never, ConnectionType.Override)] public NodePort Next;

    public string Character;
    public string Dialogue;

    public override void OnCreateConnection(NodePort from, NodePort to)
    {
        base.OnCreateConnection(from, to);
    }
}
