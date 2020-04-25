using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class UVNFStoryElements : EditorWindow
{
    public List<StoryElement> StoryElements;
    private StoryElementTypes[] storyElementCategories =
        ((StoryElementTypes[])Enum.GetValues(typeof(StoryElementTypes))).OrderBy(x => x.ToString()).ToArray();
    private bool[] elementFoldouts;

    public UVNFStoryEditor editor;
    public Vector2 ScrollPosition;

    GUIStyle storyStyle;

    [MenuItem("UVNF/Story Elements")]
    public static void Init()
    {
        UVNFStoryElements window = GetWindow<UVNFStoryElements>();
        window.Show();
    }

    public void OnGUI()
    {
        if (StoryElements == null || StoryElements[0] == null)
        {
            StoryElements = UVNFSettings.StoryElements;
            storyStyle = new GUIStyle("Box");

            Texture2D texture = new Texture2D(1, 1);
            storyStyle.normal.background = texture;
        }

        if(elementFoldouts == null || StoryElements.Count != elementFoldouts.Length)
        {
            elementFoldouts = new bool[StoryElements.Count];
            for (int i = 0; i < elementFoldouts.Length; i++) elementFoldouts[i] = true;
        }

        if(editor == null)
        {
            editor = GetWindow<UVNFStoryEditor>();
            editor.Show();
        }

        {
            Rect lastRect = new Rect();
            lastRect.position = new Vector2(lastRect.position.x + 25f, lastRect.position.y + 18f);
            lastRect.width = 2f;
            lastRect.height = position.height;
            EditorGUI.DrawRect(lastRect, Color.grey);
        }

        ScrollPosition = GUILayout.BeginScrollView(ScrollPosition, false, true);
        {
            for (int i = 0; i < storyElementCategories.Length; i++)
            {
                storyStyle = UVNFSettings.GetColorByElement(storyElementCategories[i]);
                GUIStyle buttonStyle = new GUIStyle(UVNFSettings.GetColorByElement(storyElementCategories[i]));
                buttonStyle.fixedWidth = position.width - 15f;

                bool buttonPress = GUILayout.Button((elementFoldouts[i] ? "▼" : "▲") + storyElementCategories[i].ToString(), buttonStyle);
                if (buttonPress)
                    elementFoldouts[i] = !elementFoldouts[i];

                if (elementFoldouts[i])
                {
                    StoryElement[] elementByCategory = StoryElements.Where(x => x.Type == storyElementCategories[i]).ToArray();
                    for (int j = 0; j < elementByCategory.Length; j++)
                    {
                        if (elementByCategory[j].IsVisible())
                        {
                            GUILayout.Space(5f + (i != 0 && !elementFoldouts[i] ? 4f : 0f));
                            if (GUILayout.Button(elementByCategory[j].ElementName, UVNFSettings.GetElementStyle(storyElementCategories[i])))
                                editor.AddElement(elementByCategory[j].GetType());

                            Rect lastRect = GUILayoutUtility.GetLastRect();
                            lastRect.width = 50f;
                            lastRect.height = 50f;
                            lastRect.position = new Vector2(lastRect.position.x + 5f, lastRect.position.y + 3f);

                            if (UVNFSettings.EditorSettings.ElementHints.ContainsKey(elementByCategory[j].ElementName))
                                GUI.DrawTexture(lastRect, UVNFSettings.EditorSettings.ElementHints[elementByCategory[j].ElementName], ScaleMode.ScaleToFit);
                            GUILayout.Space(10f);
                        }
                    }
                }
            }
        }
        GUILayout.EndScrollView();
    }
}
