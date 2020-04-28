using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEditor;

using XNodeEditor;
using XNode;

[CustomNodeGraphEditor(typeof(StoryGraph))]
public class StoryGraphEditor : NodeGraphEditor
{
    public override void OnOpen()
    {
        base.OnOpen();
        window.name = "Story Graph Editor";
    }

    public override string GetNodeMenuName(Type type)
    {
        if (type.BaseType == typeof(Node) || type.IsSubclassOf(typeof(Node)))
        {
            if (type.IsSubclassOf(typeof(StoryElement)))
            {
                StoryElement element = ScriptableObject.CreateInstance(type) as StoryElement;
                string returnString = element.Type.ToString() + "/" + base.GetNodeMenuName(type).Replace(" Element", "");
                ScriptableObject.DestroyImmediate(element);
                return returnString;
            }
            else
                return base.GetNodeMenuName(type).Replace("Node", "");
        }
        else return null;
    }

    public override void OnGUI()
    {
        base.OnGUI();
        if (Event.current.type == EventType.MouseDown && Event.current.button == 0 && Event.current.clickCount == 2)
        {
            CreateNode(typeof(DialogueElement), window.WindowToGridPosition(Event.current.mousePosition).OffsetY(20));
        }
    }
}