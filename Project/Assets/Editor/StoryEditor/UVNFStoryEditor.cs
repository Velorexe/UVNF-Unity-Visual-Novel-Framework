using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class UVNFStoryEditor : EditorWindowExtended
{
    public Texture2D backgroundTexture;
    public StoryContainer storyContainer;

    public bool Exported = false;

    public List<bool> storyElementsFoldout = new List<bool>();

    public int moveElement = -1;
    public bool moveUp = false;

    public int removeElement = -1;

    public int selectedIndex = 0;

    public GUIStyle style = new GUIStyle();
    public Vector2 scrollPosition = new Vector2();

    public bool standardSetup = true;

    public string storyName = "NewStory";

    [MenuItem("UVNF/Story Editor")]
    public static void Init()
    {
        UVNFStoryEditor storyWindow = GetWindow<UVNFStoryEditor>();
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
            EditorUtility.SetDirty(storyContainer);
            standardSetup = false;
        }

        GUILayout.BeginVertical(GUILayout.MinWidth(position.width), GUILayout.MinHeight(maxSize.y));
        {
            GUILayout.Label("Give your Story a relevant name and click 'Export' to get started.", EditorStyles.boldLabel);
            GUILayout.Label("Or drag and drop a Story asset onto this window.");

            storyContainer = EditorGUILayout.ObjectField("Story", storyContainer, typeof(StoryContainer), false) as StoryContainer;

            if (storyContainer != null)
            {
                GUILayout.Label("Add Element", EditorStyles.boldLabel);
                GUILayout.BeginHorizontal();
                {
                    selectedIndex = EditorGUILayout.Popup(selectedIndex, UVNFSettings.StoryElementNames);
                    if (GUILayout.Button("+", GUILayout.MaxWidth(40)))
                    {
                        //Activator.CreateInstance(UDSFSettings.StoryElements[selectedIndex].GetType()) as StoryElement
                        //storyContainer.StoryElements.Add(CreateInstance(UDSFSettings.StoryElements[selectedIndex].GetType()) as StoryElement);
                        StoryElement newElement = CreateInstance(UVNFSettings.StoryElements[selectedIndex].GetType()) as StoryElement;
                        newElement.name = newElement.ElementName;

                        EditorUtility.SetDirty(newElement);

                        AssetDatabase.AddObjectToAsset(newElement, storyContainer);
                        storyElementsFoldout.Add(true);

                        storyContainer.StoryElements.Add(newElement);
                        AssetDatabase.SaveAssets();
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

                scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.MaxHeight(position.height - 115f));
                {
                    if (storyContainer.StoryElements.Count > 0)
                    {
                        for (int i = 0; i < storyContainer.StoryElements.Count; i++)
                        {
                            storyContainer.StoryElements[i].Active = false;
                            if (storyElementsFoldout[i])
                            {
                                GUILayout.Space(55f);
                                GUILayout.BeginHorizontal();
                                {
                                    GUILayout.Space(40f);
                                    GUILayout.BeginVertical(UVNFSettings.EditorSettings.DVNFSkin.box, GUILayout.MaxWidth(700));
                                    {
                                        ChangeBackgroundStyle(storyContainer.StoryElements[i].DisplayColor);
                                        GUILayout.BeginVertical(style);
                                        {
                                            GUILayout.Space(20);
                                            Rect lastRect = GUILayoutUtility.GetLastRect();
                                            storyContainer.StoryElements[i].DisplayLayout(GUILayoutUtility.GetLastRect());
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

                            GUIStyle buttonStyle = new GUIStyle(UVNFSettings.GetElementStyle(storyContainer.StoryElements[i].Type));
                            if (storyContainer.StoryElements[i].Active)
                                buttonStyle.normal.textColor = UVNFSettings.EditorSettings.ActiveElementColor;

                            if (!storyElementsFoldout[i])
                            {
                                GUILayout.Space(5f + (i != 0 && !storyElementsFoldout[i - 1] ? 4f : 0f));
                                if (GUILayout.Button(storyContainer.StoryElements[i].ElementName, buttonStyle))
                                {
                                    storyElementsFoldout[i] = !storyElementsFoldout[i];
                                }
                                Rect lastRect = GUILayoutUtility.GetLastRect();
                                lastRect.width = 50f;
                                lastRect.height = 50f;
                                lastRect.position = new Vector2(lastRect.position.x + 5f, lastRect.position.y + 3f);

                                if (UVNFSettings.EditorSettings.ElementHints.ContainsKey(storyContainer.StoryElements[i].ElementName))
                                    GUI.DrawTexture(lastRect, UVNFSettings.EditorSettings.ElementHints[storyContainer.StoryElements[i].ElementName]);
                            }
                            else
                            {
                                Rect lastRect = GUILayoutUtility.GetLastRect();
                                lastRect.position = new Vector2(lastRect.position.x, lastRect.position.y - 40f);
                                if (GUI.Button(lastRect, storyContainer.StoryElements[i].ElementName, buttonStyle))
                                {
                                    storyElementsFoldout[i] = !storyElementsFoldout[i];
                                }
                                lastRect.width = 50f;
                                lastRect.height = 50f;
                                lastRect.position = new Vector2(lastRect.position.x + 5f, lastRect.position.y + 3f);

                                if(UVNFSettings.EditorSettings.ElementHints.ContainsKey(storyContainer.StoryElements[i].ElementName))
                                    GUI.DrawTexture(lastRect, UVNFSettings.EditorSettings.ElementHints[storyContainer.StoryElements[i].ElementName]);
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

                                storyContainer.ConnectStoryElements();
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
                            AssetDatabase.RemoveObjectFromAsset(storyContainer.StoryElements[removeElement]);
                            AssetDatabase.SaveAssets();
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
