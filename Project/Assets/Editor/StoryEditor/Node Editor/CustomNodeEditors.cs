using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;
using XNodeEditor;
using UnityEditor;

public class CustomNodeEditors : MonoBehaviour
{
    [CustomNodeEditor(typeof(StartElement))]
    public class StartNodeEditor : NodeEditor
    {
        StartElement node;

        public override void OnCreate()
        {
            if (node == null) node = target as StartElement;
            EditorUtility.SetDirty(node);
        }

        public override void OnBodyGUI()
        {
            if (!node.IsRoot)
                NodeEditorGUILayout.AddPortField(node.GetInputPort("PreviousNode"));
            NodeEditorGUILayout.AddPortField(node.GetOutputPort("NextNode"));

            GUILayout.BeginHorizontal();
            {
                GUILayout.Label("Story Name:");
                node.StoryName = EditorGUILayout.TextField(node.StoryName);
            }
            GUILayout.EndHorizontal();

            node.IsRoot = GUILayout.Toggle(node.IsRoot, "Is Root");
        }
    }

    [CustomNodeEditor(typeof(ConditionElement))]
    public class ConditionNodeEditor : NodeEditor
    {
        ConditionElement node;
        private string[] booleanOptions = new string[] { "False", "True" };

        public override void OnCreate()
        {
            if (node == null) node = target as ConditionElement;
            EditorUtility.SetDirty(node);
        }

        public override void OnBodyGUI()
        {
            NodeEditorGUILayout.AddPortField(node.GetInputPort("PreviousNode"));
            NodeEditorGUILayout.AddPortField(node.GetOutputPort("NextNode"));

            node.Variables = EditorGUILayout.ObjectField("Variables", node.Variables, typeof(VariableManager), false) as VariableManager;
            if(node.Variables != null)
            {
                node.VariableIndex = EditorGUILayout.Popup(node.VariableIndex, node.Variables.VariableNames());
                switch (node.Variables.Variables[node.VariableIndex].ValueType)
                {
                    case VariableTypes.Number:
                        node.NumberValue = EditorGUILayout.FloatField("Value", node.NumberValue); break;
                    case VariableTypes.String:
                        node.TextValue = EditorGUILayout.TextField("Value", node.TextValue); break;
                    case VariableTypes.Boolean:
                        node.BooleanValue = System.Convert.ToBoolean(EditorGUILayout.Popup("Value", System.Convert.ToInt32(node.BooleanValue), booleanOptions)); break;
                }

                GUILayout.Label("Condition Fails", EditorStyles.boldLabel);
                NodeEditorGUILayout.AddPortField(node.GetOutputPort("ConditionFails"));
            }
        }
    }

    [CustomNodeEditor(typeof(AudioDialogueElement))]
    public class AudioDialogueNodeEditor : NodeEditor
    {
        AudioDialogueElement node;

        public override void OnCreate()
        {
            if (node == null) node = target as AudioDialogueElement;
            EditorUtility.SetDirty(node);
        }

        public override void OnBodyGUI()
        {
            NodeEditorGUILayout.AddPortField(node.GetInputPort("PreviousNode"));
            NodeEditorGUILayout.AddPortField(node.GetOutputPort("NextNode"));

            node.DisplayLayout(GUILayoutUtility.GetLastRect());
        }
    }

    [CustomNodeEditor(typeof(EnterSceneElement))]
    public class EnterSceneNodeEditor : NodeEditor
    {
        EnterSceneElement node;

        public override void OnCreate()
        {
            if (node == null) node = target as EnterSceneElement;
            EditorUtility.SetDirty(node);
        }

        public override void OnBodyGUI()
        {
            NodeEditorGUILayout.AddPortField(node.GetInputPort("PreviousNode"));
            NodeEditorGUILayout.AddPortField(node.GetOutputPort("NextNode"));

            node.DisplayLayout(GUILayoutUtility.GetLastRect());
        }
    }

    [CustomNodeEditor(typeof(AffectVariableElement))]
    public class AffectVariableNodeEditor : NodeEditor
    {
        AffectVariableElement node;

        public override void OnCreate()
        {
            if (node == null) node = target as AffectVariableElement;
            EditorUtility.SetDirty(node);
        }

        public override void OnBodyGUI()
        {
            NodeEditorGUILayout.AddPortField(node.GetInputPort("PreviousNode"));
            NodeEditorGUILayout.AddPortField(node.GetOutputPort("NextNode"));

            node.DisplayLayout(GUILayoutUtility.GetLastRect());
        }
    }
}
