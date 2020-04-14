using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

[NodeTint("#FEECCE"), Serializable]
public class BranchNode : Node
{
    [Input(ShowBackingValue.Never, ConnectionType.Override)]
    public Empty Previous;
    [Output(ShowBackingValue.Never, ConnectionType.Override)]
    public Empty Next;


}
