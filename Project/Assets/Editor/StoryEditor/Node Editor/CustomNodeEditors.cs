using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;
using XNodeEditor;
using UnityEditor;

public class CustomNodeEditors : MonoBehaviour
{
    //[CustomNodeEditor(typeof(StartElement))]
    //public class StartNodeEditor : NodeEditor
    //{
    //    StartElement node;

    //    public override void OnCreate()
    //    {
    //        if (node == null) node = target as StartElement;
    //        EditorUtility.SetDirty(node);
    //    }

    //    public override void OnBodyGUI()
    //    {
    //        if (!node.IsRoot)
    //            NodeEditorGUILayout.AddPortField(node.GetInputPort("PreviousNode"));
    //        NodeEditorGUILayout.AddPortField(node.GetOutputPort("NextNode"));

    //        GUILayout.BeginHorizontal();
    //        {
    //            GUILayout.Label("Story Name:");
    //            node.StoryName = EditorGUILayout.TextField(node.StoryName);
    //        }
    //        GUILayout.EndHorizontal();

    //        node.IsRoot = GUILayout.Toggle(node.IsRoot, "Is Root");
    //    }
    //}

    //[CustomNodeEditor(typeof(ConditionElement))]
    //public class ConditionNodeEditor : NodeEditor
    //{
    //    ConditionElement node;
    //    private string[] booleanOptions = new string[] { "False", "True" };

    //    public override void OnCreate()
    //    {
    //        if (node == null) node = target as ConditionElement;
    //        EditorUtility.SetDirty(node);
    //    }

    //    public override void OnBodyGUI()
    //    {
    //        NodeEditorGUILayout.AddPortField(node.GetInputPort("PreviousNode"));
    //        NodeEditorGUILayout.AddPortField(node.GetOutputPort("NextNode"));

    //        node.Variables = EditorGUILayout.ObjectField("Variables", node.Variables, typeof(VariableManager), false) as VariableManager;
    //        if (node.Variables != null)
    //        {
    //            node.VariableIndex = EditorGUILayout.Popup(node.VariableIndex, node.Variables.VariableNames());
    //            switch (node.Variables.Variables[node.VariableIndex].ValueType)
    //            {
    //                case VariableTypes.Number:
    //                    node.NumberValue = EditorGUILayout.FloatField("Value", node.NumberValue); break;
    //                case VariableTypes.String:
    //                    node.TextValue = EditorGUILayout.TextField("Value", node.TextValue); break;
    //                case VariableTypes.Boolean:
    //                    node.BooleanValue = System.Convert.ToBoolean(EditorGUILayout.Popup("Value", System.Convert.ToInt32(node.BooleanValue), booleanOptions)); break;
    //            }

    //            GUILayout.Label("Condition Fails", EditorStyles.boldLabel);
    //            NodeEditorGUILayout.AddPortField(node.GetOutputPort("ConditionFails"));
    //        }
    //    }
    //}

    [CustomNodeEditor(typeof(ChoiceElement))]
    public class ChoiceNodeEditor : NodeEditor
    {
        ChoiceElement node;
        bool foldout = true;

        public override void OnCreate()
        {
            if (node == null) node = target as ChoiceElement;
            EditorUtility.SetDirty(node);
            ReplaceTint(node.DisplayColor);
        }

        public override void OnHeaderGUI()
        {
            if (DisplayElementType(node.Type, node.ElementName, GetWidth()))
                foldout = !foldout;
        }

        public override void OnBodyGUI()
        {
            GUILayout.BeginHorizontal();
            {
                GUILayout.Label("Previous", EditorStyles.boldLabel);
                NodeEditorGUILayout.AddPortField(node.GetInputPort("PreviousNode"));
                GUILayout.Space(170f);
            }
            GUILayout.EndHorizontal();

            if (foldout)
            {
                for (int i = 0; i < node.Choices.Count; i++)
                {
                    node.Choices[i] = GUILayout.TextField(node.Choices[i]);
                    NodeEditorGUILayout.AddPortField(node.GetOutputPort("Choice" + i));

                    if (GUILayout.Button("-"))
                        node.RemoveChoice(i);

                    GUILayout.Space(7.5f);
                }

                if (GUILayout.Button("+"))
                    node.AddChoice();

                node.ShuffleChocies = GUILayout.Toggle(node.ShuffleChocies, "Shuffle Choices");
                node.HideDialogue = GUILayout.Toggle(node.HideDialogue, "Hide Dialogue");
            }
            else
            {
                for (int i = 0; i < node.Choices.Count; i++)
                {
                    GUILayout.Label("");
                    NodeEditorGUILayout.AddPortField(node.GetOutputPort("Choice" + i));
                }
            }
        }
    }

    [CustomNodeEditor(typeof(StoryElement))]
    public class StoryElementNodeEditor : NodeEditor
    {
        StoryElement node;
        bool foldout = true;

        public override void OnCreate()
        {
            if (node == null) node = target as StoryElement;
            EditorUtility.SetDirty(node);

            ReplaceTint(node.DisplayColor);
        }

        public override void OnHeaderGUI()
        {
            if (DisplayElementType(node.Type, node.ElementName, GetWidth()))
                foldout = !foldout;
        }

        public override void OnBodyGUI()
        {
            Rect lastRect;
            GUILayout.BeginHorizontal();
            {
                GUILayout.Label("Previous", EditorStyles.boldLabel);
                NodeEditorGUILayout.AddPortField(node.GetInputPort("PreviousNode"));
                GUILayout.Space(170f);
                GUILayout.Label("Next", EditorStyles.boldLabel);
                NodeEditorGUILayout.AddPortField(node.GetOutputPort("NextNode"));
                lastRect = GUILayoutUtility.GetLastRect();
            }
            GUILayout.EndHorizontal();

            if (foldout)
            {
                node.DisplayNodeLayout(lastRect);
            }
        }
    }

    public static bool DisplayElementType(StoryElementTypes type, string elementName, int width)
    {
        bool result = GUI.Button(new Rect(0f, 0f, width + 50f, 60f), elementName, UVNFSettings.GetElementStyle(type));

        if (UVNFSettings.EditorSettings.ElementHints.ContainsKey(elementName))
            GUI.DrawTexture(new Rect(5f, 5f, 50f, 50f), UVNFSettings.EditorSettings.ElementHints[elementName]);

        GUILayout.Space(60f);
        return result;
    }
}
