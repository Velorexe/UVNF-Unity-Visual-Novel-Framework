using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using System;
using System.Linq;
using System.Reflection;

[Serializable]
public class UDSFStoryEditor : EditorWindowExtended
{
    public Texture2D backgroundTexture;
    public StoryContainer storyContainer;

    public List<bool> storyElementsFoldout = new List<bool>();

    public int moveElement = -1;
    public bool moveUp = false;

    public int removeElement = -1;

    public int selectedIndex = 0;

    public GUIStyle style = new GUIStyle();
    public Vector2 scrollPosition = new Vector2();

    public bool standardSetup = true;

    public string storyName = "NewStory";

    [MenuItem("UDSF/Story Editor")]
    public static void Init()
    {
        UDSFStoryEditor storyWindow = GetWindow<UDSFStoryEditor>();
        storyWindow.Show();
    }

    private void SetStandardBackground()
    {
        backgroundTexture = new Texture2D(1, 1, TextureFormat.RGBA32, false);
        backgroundTexture.SetPixel(0, 0, new Color(1f, 0.90625f, 0.90625f));
        backgroundTexture.Apply();
    }

    private void OnGUI()
    {
        if (standardSetup)
        {
            SetStandardBackground();
            storyContainer = CreateInstance<StoryContainer>();
            standardSetup = false;
        }

        GUI.DrawTexture(new Rect(0, 0, maxSize.x, maxSize.y), backgroundTexture, ScaleMode.StretchToFill);
        GUILayout.BeginVertical(GUILayout.MinWidth(position.width), GUILayout.MinHeight(maxSize.y));
        {
            GUILayout.Label("Create a story here.", EditorStyles.boldLabel);
            GUILayout.BeginHorizontal();
            {
                GUILayout.Label("Story Name: ", GUILayout.MaxWidth(80));
                storyName = GUILayout.TextField(storyName);
            }
            GUILayout.EndHorizontal();
            if (GUILayout.Button("Export"))
            {
                ExportStory();
            }

            GUILayout.Label("Add Element", EditorStyles.boldLabel);
            GUILayout.BeginHorizontal();
            {
                selectedIndex = EditorGUILayout.Popup(selectedIndex, UDSFSettings.StoryElementNames);
                if (GUILayout.Button("+", GUILayout.MaxWidth(40)))
                {
                    storyContainer.StoryElements.Add(Activator.CreateInstance(UDSFSettings.StoryElements[selectedIndex].GetType()) as StoryElement);
                    storyElementsFoldout.Add(true);
                }
            }
            GUILayout.EndHorizontal();

            {
                Rect lastRect = GUILayoutUtility.GetLastRect();
                lastRect.position = new Vector2(lastRect.position.x + 20f, lastRect.position.y + 18f);
                lastRect.width = 2f;
                lastRect.height = position.height;
                EditorGUI.DrawRect(lastRect, Color.grey);
            }

            scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.MaxHeight(position.height - 80));
            {
                if (storyContainer.StoryElements.Count > 0)
                {
                    for (int i = 0; i < storyContainer.StoryElements.Count; i++)
                    {
                        if (storyElementsFoldout[i])
                        {
                            GUILayout.Space(55f);
                            GUILayout.BeginHorizontal();
                            {
                                GUILayout.Space(40f);
                                GUILayout.BeginVertical(UDSFSettings.Settings.BoxGUIStyle, GUILayout.MaxWidth(position.size.x));
                                {
                                    ChangeBackgroundStyle(storyContainer.StoryElements[i].DisplayColor);
                                    GUILayout.BeginVertical(style);
                                    {
                                        GUILayout.Space(20);
                                        storyContainer.StoryElements[i].DisplayLayout();
                                        GUILayout.Label("", GUI.skin.horizontalSlider);

                                        GUILayout.BeginHorizontal();
                                        {
                                            if (GUILayout.Button("▲"))
                                            {
                                                moveElement = i;
                                                moveUp = true;
                                            }
                                            else if (GUILayout.Button("▼"))
                                            {
                                                moveElement = i;
                                                moveUp = false;
                                            }
                                        }
                                        GUILayout.EndHorizontal();
                                        if (GUILayout.Button("-"))
                                        {
                                            removeElement = i;
                                        }
                                    }
                                    GUILayout.EndVertical();
                                }
                                GUILayout.EndVertical();
                            }
                            GUILayout.EndHorizontal();
                        }
                        else
                        {
                            GUILayout.Space(10f);
                        }

                        if (!storyElementsFoldout[i])
                        {
                            GUILayout.Space(5f);
                            if (GUILayout.Button(storyContainer.StoryElements[i].ElementName, UDSFSettings.Settings.GetElementStyle(storyContainer.StoryElements[i].Type)))
                            {
                                storyElementsFoldout[i] = !storyElementsFoldout[i];
                            }
                            Rect lastRect = GUILayoutUtility.GetLastRect();
                            lastRect.width = 50f;
                            lastRect.height = 50f;
                            lastRect.position = new Vector2(lastRect.position.x + 5f, lastRect.position.y + 3f);
                            GUI.DrawTexture(lastRect, UDSFSettings.Settings.DialogueElementTexture);
                        }
                        else
                        {
                            Rect lastRect = GUILayoutUtility.GetLastRect();
                            lastRect.position = new Vector2(lastRect.position.x, lastRect.position.y - 40f);
                            if (GUI.Button(lastRect, storyContainer.StoryElements[i].ElementName, UDSFSettings.Settings.GetElementStyle(storyContainer.StoryElements[i].Type)))
                            {
                                storyElementsFoldout[i] = !storyElementsFoldout[i];
                            }
                            lastRect.width = 50f;
                            lastRect.height = 50f;
                            lastRect.position = new Vector2(lastRect.position.x + 5f, lastRect.position.y + 3f);
                            GUI.DrawTexture(lastRect, UDSFSettings.Settings.DialogueElementTexture);
                        }
                    }
                    if (moveElement != -1)
                    {
                        if (moveUp && moveElement > 0)
                        {
                            StoryElement shiftElement = storyContainer.StoryElements[moveElement - 1];
                            storyContainer.StoryElements[moveElement - 1] = storyContainer.StoryElements[moveElement];
                            storyContainer.StoryElements[moveElement] = shiftElement;

                            bool shiftFoldout = storyElementsFoldout[moveElement - 1];
                            storyElementsFoldout[moveElement - 1] = storyElementsFoldout[moveElement];
                            storyElementsFoldout[moveElement] = shiftFoldout;
                        }
                        else if (moveElement < storyContainer.StoryElements.Count - 1)
                        {
                            StoryElement shiftElement = storyContainer.StoryElements[moveElement + 1];
                            storyContainer.StoryElements[moveElement + 1] = storyContainer.StoryElements[moveElement];
                            storyContainer.StoryElements[moveElement] = shiftElement;

                            bool shiftFoldout = storyElementsFoldout[moveElement + 1];
                            storyElementsFoldout[moveElement + 1] = storyElementsFoldout[moveElement];
                            storyElementsFoldout[moveElement] = shiftFoldout;
                        }

                        moveElement = -1;
                    }

                    if (removeElement != -1)
                    {
                        storyContainer.StoryElements.RemoveAt(removeElement);
                        storyElementsFoldout.RemoveAt(removeElement);
                        removeElement = -1;
                    }
                }
            }
            GUILayout.EndScrollView();
        }
        GUILayout.EndVertical();
    }

    private void ExportStory()
    {
        if (!AssetDatabase.IsValidFolder("Assets/Story"))
            AssetDatabase.CreateFolder("Assets", "Story");
        AssetDatabase.CreateAsset(storyContainer, $"Assets/Story/{storyName}.asset");
        AssetDatabase.SaveAssets();
    }

    private void ChangeBackgroundStyle(Color color)
    {
        Texture2D backgroundTexture = new Texture2D(1, 1);
        backgroundTexture.SetPixel(0, 0, color);
        backgroundTexture.Apply();
        style.normal.background = backgroundTexture;
    }
}
