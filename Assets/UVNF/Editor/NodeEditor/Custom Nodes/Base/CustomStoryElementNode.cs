using UnityEditor;
using UnityEngine;
using UVNF.Core.Story;
using UVNF.Editor.Settings;
using XNodeEditor;

namespace UVNF.Editor.Story.Nodes
{
    [CustomNodeEditor(typeof(StoryElement))]
    public class CustomStoryElementNode : NodeEditor
    {
        internal StoryElement Node;
        internal bool Foldout = true;

        public override void OnCreate()
        {
            if (Node == null) Node = target as StoryElement;
            EditorUtility.SetDirty(Node);
        }

        public override void OnHeaderGUI()
        {
            DisplayElementType(Node.Type, Node.ElementName, GetWidth());
        }

        public virtual void DrawBody()
        {
            base.OnBodyGUI();
        }

        public override void OnBodyGUI()
        {
            RenderNodeConnections();

            if (Foldout)
            {
                DrawBody();
                GUILayout.Space(EditorGUIUtility.singleLineHeight);
            }

            RenderFoldout();
        }

        internal void RenderNodeConnections(bool renderPrevious = true, bool renderNext = true)
        {
            GUILayout.BeginHorizontal();
            {
                if (renderPrevious)
                {
                    GUILayout.Label("Previous", EditorStyles.boldLabel);
                    NodeEditorGUILayout.AddPortField(Node.GetInputPort("PreviousNode"));
                }

                GUILayout.Space(170f);

                if (renderNext)
                {
                    GUILayout.Label("Next", EditorStyles.boldLabel);
                    NodeEditorGUILayout.AddPortField(Node.GetOutputPort("NextNode"));
                }
            }
            GUILayout.EndHorizontal();
        }

        internal void RenderFoldout()
        {
            GUIContent arrow = Foldout ? EditorGUIUtility.IconContent("d_Toolbar Minus") : EditorGUIUtility.IconContent("d_Toolbar Plus");

            if (GUILayout.Button(arrow))
                Foldout = !Foldout;
        }

        internal void DisplayElementType(StoryElementTypes type, string elementName, int width)
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