using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;
using XNodeEditor;
using UnityEditor;
using UVNF.Core.Story;
using UVNF.Core.Story.Dialogue;
using UVNF.Editor.Settings;

namespace UVNF.Editor.Story.Nodes
{
    public class CustomNodeEditors : MonoBehaviour
    {
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
}