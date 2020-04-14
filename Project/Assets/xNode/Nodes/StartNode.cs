using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

[NodeTint("#CCFCC3"), Serializable]
public class StartNode : Node
{
    [Output(ShowBackingValue.Always, ConnectionType.Override)]
    public Empty Next;
}
