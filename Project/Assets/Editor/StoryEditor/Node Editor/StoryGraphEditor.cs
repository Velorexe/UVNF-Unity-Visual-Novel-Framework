﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNodeEditor;
using XNode;

[CustomNodeGraphEditor(typeof(StoryGraph))]
public class StoryGraphEditor : NodeGraphEditor
{
    public override string GetNodeMenuName(Type type)
    {
        if (type.BaseType == typeof(Node))
            return base.GetNodeMenuName(type).Replace("Node", "");
        else return null;
    }

    public override void OnGUI()
    {
        base.OnGUI();
        if (Event.current.type == EventType.MouseDown && Event.current.button == 0 && Event.current.clickCount == 2)
        {
            CreateNode(typeof(DialogueNode), window.WindowToGridPosition(Event.current.mousePosition).OffsetY(10));
        }
    }
}
