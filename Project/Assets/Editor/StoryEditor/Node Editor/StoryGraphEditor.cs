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

[CustomNodeEditor(typeof(ChoiceElement))]
public class BranchNodeEditor : NodeEditor
{
    private ChoiceElement branchNode;

    public override void OnCreate()
    {
        base.OnCreate();
        if (branchNode == null) branchNode = target as ChoiceElement;
        EditorUtility.SetDirty(branchNode);

        for (int i = 0; i < branchNode.Choices.Count; i++)
            branchNode.AddDynamicOutput(typeof(Empty), Node.ConnectionType.Override, Node.TypeConstraint.None, i + "");
    }

    public override void OnBodyGUI()
    {
        NodeEditorGUILayout.AddPortField(branchNode.GetInputPort("PreviousNode"));
        IEnumerable<NodePort> outputFields = branchNode.DynamicOutputs;

        int removeIndex = -1;
        for (int i = 0; i < branchNode.Choices.Count; i++)
        {
            GUILayout.BeginHorizontal();
            {
                branchNode.Choices[i] = GUILayout.TextField(branchNode.Choices[i]);
                if (GUILayout.Button("-", GUILayout.MaxWidth(20)))
                    removeIndex = i;
            }
            GUILayout.EndHorizontal();
            NodeEditorGUILayout.AddPortField(outputFields.ElementAt(i));

            GUILayout.Space(11f);
        }

        if (removeIndex != -1)
        {
            branchNode.Choices.RemoveAt(removeIndex);
            branchNode.RemoveDynamicPort("" + removeIndex);

            for (int i = 0; i < outputFields.Count(); i++)
                outputFields.ElementAt(i).fieldName = i + "";
        }

        if (GUILayout.Button("+"))
        {
            branchNode.Choices.Add("");
            NodePort debug = branchNode.AddDynamicOutput(typeof(Empty), Node.ConnectionType.Override, Node.TypeConstraint.None, "" + (branchNode.DynamicOutputs.Count() - 1));
        }
    }
}

[CustomNodeEditor(typeof(DialogueElement))]
public class DialogueNodeEditor : NodeEditor
{
    private DialogueElement dialogueNode;
    private GUIStyle richDialogue;

    public override void OnCreate()
    {
        base.OnCreate();
        if (dialogueNode == null) dialogueNode = target as DialogueElement;
        EditorUtility.SetDirty(dialogueNode);

        richDialogue = new GUIStyle("TextArea");
        richDialogue.normal.textColor = Color.black;
        richDialogue.richText = true;
    }

    public override void OnBodyGUI()
    {
        NodeEditorGUILayout.AddPortField(dialogueNode.GetInputPort("PreviousNode"));
        NodeEditorGUILayout.AddPortField(dialogueNode.GetOutputPort("NextNode"));
        dialogueNode.DisplayLayout(GUILayoutUtility.GetLastRect());
    }
}

[CustomNodeEditor(typeof(StartNode))]
public class StartNodeEditor : NodeEditor
{
    private StartNode startNode;

    public override void OnCreate()
    {
        base.OnCreate();
        if (startNode == null) startNode = target as StartNode;
        EditorUtility.SetDirty(startNode);
    }

    public override void OnBodyGUI()
    {
        if (!startNode.IsRoot)
            NodeEditorGUILayout.AddPortField(startNode.GetInputPort("Previous"));
        NodeEditorGUILayout.AddPortField(startNode.GetOutputPort("Next"));

        startNode.IsRoot = GUILayout.Toggle(startNode.IsRoot, "Is Root");
    }
}