using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

[NodeTint("#FEC4C4"), Serializable]
public class DialogueNode : Node
{
    [Input] public Empty Previous;
    [Output] public Empty Next;

    [TextArea(10, 12)]
    public string Dialogue;
}
