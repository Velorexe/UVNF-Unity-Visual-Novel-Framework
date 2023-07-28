using UnityEditor;
using UnityEngine;
using UVNF.Core.Story.Dialogue;

namespace UVNF.Editor.Story.Nodes
{
    [CustomNodeEditor(typeof(ChoiceElement))]
    public class CustomChoiceElementNode : CustomStoryElementNode
    {
        public override void OnBodyGUI()
        {
            RenderNodeConnections(renderNext: false);

            if (Foldout)
            {
                DrawBody();
                GUILayout.Space(EditorGUIUtility.singleLineHeight);
            }

            RenderFoldout();
        }
    }
}
