using UnityEditor;
using UnityEngine;
using UVNF.Core.Story;
using UVNF.Core.Story.Dialogue;
using UVNF.Editor.Settings;
using XNodeEditor;

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
            }

            public override void OnHeaderGUI()
            {
                DisplayElementType(node.Type, node.ElementName, GetWidth());
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



        public static void DisplayElementType(StoryElementTypes type, string elementName, int width)
        {
            GUI.DrawTexture(new Rect(5f, 5f, width - 10f, 36f), UVNFSettings.GetElementStyle(type).normal.background);

            GUILayout.Space(5f);
            GUILayout.BeginHorizontal();
            {
                GUILayout.Space(23f);
                GUILayout.Label(elementName, UVNFSettings.GetLabelStyle(type));
            }
            GUILayout.EndHorizontal();

            if (UVNFSettings.EditorSettings.ElementHints.ContainsKey(elementName))
                GUI.DrawTexture(new Rect(5f, 7f, 32f, 32f), UVNFSettings.EditorSettings.ElementHints[elementName]);
        }
    }
}