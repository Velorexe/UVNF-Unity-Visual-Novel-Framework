using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

[NodeTint("#CCFCC3"), Serializable]
public class StartNode : Node
{
    [Input(ShowBackingValue.Never, ConnectionType.Override)]
    public NodePort Previous;
    [Output(ShowBackingValue.Never, ConnectionType.Override)]
    public NodePort Next;

    public bool IsRoot;
}
