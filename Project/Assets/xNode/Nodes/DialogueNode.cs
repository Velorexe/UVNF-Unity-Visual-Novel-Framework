using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

[NodeTint("#FFFFFF"), Serializable]
public class DialogueNode : Node
{
    [Input(ShowBackingValue.Never, ConnectionType.Override)] public Empty Previous;
    [Output(ShowBackingValue.Never, ConnectionType.Override)] public Empty Next;

    [TextArea(10, 12)]
    public string Dialogue;
}
