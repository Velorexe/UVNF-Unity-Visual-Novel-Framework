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

        /// <summary>
        /// Renders the Header of the Node
        /// </summary>
        public override void OnHeaderGUI()
        {
            DisplayElementType(Node.Type, Node.ElementName, GetWidth());
        }

        /// <summary>
        /// Draws the body of the node, only rendered if <see cref="Foldout"/> is <see langword="true"/>
        /// </summary>
        public virtual void DrawBody()
        {
            base.OnBodyGUI();
        }

        /// <summary>
        /// Renders the Node's body
        /// </summary>
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

        /// <summary>
        /// Renders the Node's NodePorts
        /// </summary>
        /// <param name="renderPrevious">Set to <see langword="true"/> if the "previous" node port should be rendered</param>
        /// <param name="renderNext">Set to <see langword="true"/> if the "next" node port should be rendered</param>
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

        /// <summary>
        /// Renders the Foldout button inside the Node
        /// </summary>
        internal void RenderFoldout()
        {
            GUIContent arrow = Foldout ? EditorGUIUtility.IconContent("d_Toolbar Minus") : EditorGUIUtility.IconContent("d_Toolbar Plus");

            if (GUILayout.Button(arrow))
                Foldout = !Foldout;
        }

        /// <summary>
        /// Displays a custom header that matches the <see cref="StoryElement"/>
        /// </summary>
        /// <param name="type">The type of <see cref="StoryElement"/></param>
        /// <param name="elementName">The name of the <see cref="StoryElement"/></param>
        /// <param name="width">The width at which the Node is displayed</param>
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