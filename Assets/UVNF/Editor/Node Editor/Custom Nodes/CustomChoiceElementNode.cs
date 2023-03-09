using UnityEditor;
using UnityEngine;
using UVNF.Core.Story.Dialogue;
using XNodeEditor;

namespace UVNF.Editor.Story.Nodes
{
    [CustomNodeEditor(typeof(ChoiceElement))]
    public class CustomChoiceElementNode : CustomStoryElementNode
    {
        public override void OnBodyGUI()
        {
            ChoiceElement node = (ChoiceElement)Node;

            RenderNodeConnections(true, false);
            GUILayout.Space(EditorGUIUtility.singleLineHeight);

            if (Foldout)
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

                GUILayout.Space(EditorGUIUtility.singleLineHeight);

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

            GUILayout.Space(EditorGUIUtility.singleLineHeight);
            RenderFoldout();
        }
    }
}