using UnityEditor;
using UnityEngine;
using UVNF.Core.Story;
using UVNF.Editor.Settings;
using XNodeEditor;

namespace UVNF.Editor.Story.Nodes
{
    [CustomNodeEditor(typeof(StoryElement))]
    public class StoryElementNodeEditor : NodeEditor
    {
        StoryElement node;
        bool foldout = true;

        public override void OnCreate()
        {
            if (node == null) node = target as StoryElement;
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

                GUILayout.Label("Next", EditorStyles.boldLabel);
                NodeEditorGUILayout.AddPortField(node.GetOutputPort("NextNode"));
            }
            GUILayout.EndHorizontal();

            if (foldout)
            {
                GUILayout.Space(EditorGUIUtility.singleLineHeight);
                base.OnBodyGUI();
                GUILayout.Space(EditorGUIUtility.singleLineHeight);
            }

            GUIContent arrow = foldout ? EditorGUIUtility.IconContent("d_Toolbar Minus") : EditorGUIUtility.IconContent("d_Toolbar Plus");

            if (GUILayout.Button(arrow))
                foldout = !foldout;
        }

        public void DisplayElementType(StoryElementTypes type, string elementName, int width)
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