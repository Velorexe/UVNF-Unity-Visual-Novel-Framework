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

    [SerializeField]
    public List<StoryElement> storyElements = new List<StoryElement>();
    public string[] storyElementNames
    {
        get
        {
            if (storyElements == null)
                return new string[] { };
            return storyElements.Select(x => x.ElementName).ToArray();
        }
    }


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

    private void InitializeStoryElements()
    {
        foreach (Type type in
            Assembly.GetAssembly(typeof(StoryElement)).GetTypes()
            .Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(StoryElement))))
        {
            storyElements.Add((StoryElement)Activator.CreateInstance(type));
        }
        storyElements.Sort();
    }

    private void OnGUI()
    {
        if (standardSetup)
        {
            SetStandardBackground();
            InitializeStoryElements();
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
                selectedIndex = EditorGUILayout.Popup(selectedIndex, storyElementNames);
                if (GUILayout.Button("+", GUILayout.MaxWidth(40)))
                {
                    storyContainer.StoryElements.Add(Activator.CreateInstance(storyElements[selectedIndex].GetType()) as StoryElement);
                }
            }
            GUILayout.EndHorizontal();
            scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.MaxHeight(position.height - 80));
            {
                if (storyContainer.StoryElements.Count > 0)
                {
                    for (int i = 0; i < storyContainer.StoryElements.Count; i++)
                    {
                        GUILayout.BeginVertical("Box", GUILayout.MaxWidth(position.size.x));
                        {
                            ChangeBackgroundStyle(storyContainer.StoryElements[i].DisplayColor);
                            GUILayout.BeginVertical(style);
                            {
                                GUILayout.Label(storyContainer.StoryElements[i].ElementName, EditorStyles.boldLabel);
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

                    if (moveElement != -1)
                    {
                        if (moveUp && moveElement < storyContainer.StoryElements.Count - 2)
                        {
                            StoryElement shiftElement = storyContainer.StoryElements[moveElement + 1];
                            storyContainer.StoryElements[moveElement + 1] = storyContainer.StoryElements[moveElement];
                            storyContainer.StoryElements[moveElement] = shiftElement;
                        }
                        else if (moveElement > 0)
                        {
                            StoryElement shiftElement = storyContainer.StoryElements[moveElement - 1];
                            storyContainer.StoryElements[moveElement - 1] = storyContainer.StoryElements[moveElement];
                            storyContainer.StoryElements[moveElement] = shiftElement;
                        }

                        moveElement = -1;
                    }

                    if (removeElement != -1)
                    {
                        storyContainer.StoryElements.RemoveAt(removeElement);
                        removeElement = -1;
                    }
                }
                else
                {
                    GUILayout.Label("There are no elements in the story yet, add some to create your story!", EditorStyles.boldLabel);
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
