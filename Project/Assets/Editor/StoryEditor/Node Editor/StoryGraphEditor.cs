using System;
using UnityEngine;
using UnityEditor;

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
            CreateNode(typeof(DialogueNode), window.WindowToGridPosition(Event.current.mousePosition).OffsetY(20));
        }
    }
}

[CustomNodeEditor(typeof(BranchNode))]
public class BranchNodeEditor : NodeEditor
{
    private BranchNode branchNode;

    public override void OnCreate()
    {
        base.OnCreate();
        if (branchNode == null) branchNode = target as BranchNode;
        EditorUtility.SetDirty(branchNode);
    }

    public override void OnBodyGUI()
    {
        NodeEditorGUILayout.AddPortField(branchNode.GetInputPort("Previous"));

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
            NodeEditorGUILayout.AddPortField(branchNode.ChoicePorts[i]);

            GUILayout.Space(11f);
        }

        if (removeIndex != -1) 
        {
            branchNode.Choices.RemoveAt(removeIndex);
            branchNode.ChoicePorts.RemoveAt(removeIndex);
        }

        if (GUILayout.Button("+"))
        {
            branchNode.Choices.Add("");
            branchNode.ChoicePorts.Add(branchNode.AddDynamicOutput(typeof(Empty), Node.ConnectionType.Override));
        }
    }
}

[CustomNodeEditor(typeof(DialogueNode))]
public class DialogueNodeEditor : NodeEditor
{
    private DialogueNode dialogueNode;
    private GUIStyle richDialogue;

    public override void OnCreate()
    {
        base.OnCreate();
        if (dialogueNode == null) dialogueNode = target as DialogueNode;
        EditorUtility.SetDirty(dialogueNode);

        richDialogue = new GUIStyle("TextArea");
        richDialogue.normal.textColor = Color.black;
        richDialogue.richText = true;
    }

    public override void OnBodyGUI()
    {    
        NodeEditorGUILayout.AddPortField(dialogueNode.GetInputPort("Previous"));
        NodeEditorGUILayout.AddPortField(dialogueNode.GetOutputPort("Next"));

        GUILayout.BeginHorizontal();
        {
            GUILayout.Label("Character:", GUILayout.MaxWidth(65));
            dialogueNode.Character = GUILayout.TextField(dialogueNode.Character);
        }
        GUILayout.EndHorizontal();
        dialogueNode.Dialogue = GUILayout.TextArea(dialogueNode.Dialogue, richDialogue);
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
        if(!startNode.IsRoot)
            NodeEditorGUILayout.AddPortField(startNode.GetInputPort("Previous"));
        NodeEditorGUILayout.AddPortField(startNode.GetOutputPort("Next"));

        startNode.IsRoot = GUILayout.Toggle(startNode.IsRoot, "Is Root");
    }
}