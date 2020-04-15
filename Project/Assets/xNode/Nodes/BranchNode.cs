using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

[NodeTint("#FEECCE"), Serializable]
public class BranchNode : Node
{
    [Input(ShowBackingValue.Never, ConnectionType.Override)]
    public NodePort Previous;

    public List<string> Choices = new List<string>();
    public List<NodePort> ChoicePorts = new List<NodePort>();
}
