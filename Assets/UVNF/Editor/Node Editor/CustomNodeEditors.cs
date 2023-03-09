using UnityEngine;
using UVNF.Core.Story;
using UVNF.Editor.Settings;

namespace UVNF.Editor.Story.Nodes
{
    public class CustomNodeEditors : MonoBehaviour
    {
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