using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

[NodeTint("#FEECCE"), Serializable]
public class BranchNode : Node
{
    [Input]
    public Empty Previous;
    [Output]
    public Empty Next;
}
