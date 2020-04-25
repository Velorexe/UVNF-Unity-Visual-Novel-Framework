using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class UVNFStoryEditor : EditorWindowExtended
{
    public Texture2D backgroundTexture;

    public StoryGraph storyContainer;
    public List<StoryElement> currentElements = new List<StoryElement>();

    public bool Exported = false;

    public List<bool> storyElementsFoldout = new List<bool>();

    public int moveElement = -1;
    public bool moveUp = false;

    public int removeElement = -1;

    public int storyIndex = 0;
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

        UVNFStoryElements elementWindow = GetWindow<UVNFStoryElements>();
        elementWindow.editor = GetWindow<UVNFStoryEditor>();
        elementWindow.Show();
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
            storyContainer = CreateInstance<StoryGraph>();
            EditorUtility.SetDirty(storyContainer);
            standardSetup = false;
        }
        
        if(storyContainer != null && storyElementsFoldout.Count != currentElements.Count)
        {
            for (int i = 0; i < currentElements.Count; i++)
            {
                storyElementsFoldout.Add(true);
            }
        }


        GUILayout.BeginVertical(GUILayout.MinWidth(position.width), GUILayout.MinHeight(maxSize.y));
        {
            GUILayout.Label("Give your Story a relevant name and click 'Export' to get started.", EditorStyles.boldLabel);
            GUILayout.Label("Or drag and drop a Story asset onto this window.");

            storyContainer = EditorGUILayout.ObjectField("Story", storyContainer, typeof(StoryGraph), false) as StoryGraph;

            if (storyContainer != null)
            {
                GUILayout.Label("Select Sub-story", EditorStyles.boldLabel);
                GUILayout.BeginHorizontal();
                {
                    storyIndex = EditorGUILayout.Popup(storyIndex, storyContainer.StoryNames);
                    storyContainer.RefreshStories();
                    currentElements = storyContainer.ShortStory(storyIndex);
                }
                GUILayout.EndHorizontal();
                {
                    Rect lastRect = GUILayoutUtility.GetLastRect();
                    lastRect.position = new Vector2(lastRect.position.x + 20f, lastRect.position.y + 18f);
                    lastRect.width = 2f;
                    lastRect.height = position.height;
                    EditorGUI.DrawRect(lastRect, Color.grey);
                }

                scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.MaxHeight(position.height - 100f));
                {
                    if (currentElements.Count > 0)
                    {
                        for (int i = 0; i < currentElements.Count; i++)
                        {
                            currentElements[i].Active = false;
                            if (currentElements[i].IsVisible())
                            {
                                if (storyElementsFoldout[i])
                                {
                                    GUILayout.Space(55f);
                                    GUILayout.BeginHorizontal();
                                    {
                                        GUILayout.Space(40f);
                                        GUILayout.BeginVertical(UVNFSettings.EditorSettings.DVNFSkin.box, GUILayout.MaxWidth(700));
                                        {
                                            ChangeBackgroundStyle(currentElements[i].DisplayColor);
                                            GUILayout.BeginVertical(style);
                                            {
                                                GUILayout.Space(20);
                                                Rect lastRect = GUILayoutUtility.GetLastRect();
                                                currentElements[i].DisplayLayout(GUILayoutUtility.GetLastRect());
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

                                GUIStyle buttonStyle = new GUIStyle(UVNFSettings.GetElementStyle(currentElements[i].Type));
                                if (currentElements[i].Active)
                                    buttonStyle.normal.textColor = UVNFSettings.EditorSettings.ActiveElementColor;

                                if (!storyElementsFoldout[i])
                                {
                                    GUILayout.Space(5f + (i != 0 && !storyElementsFoldout[i - 1] ? 4f : 0f));
                                    if (GUILayout.Button(currentElements[i].ElementName, buttonStyle))
                                    {
                                        storyElementsFoldout[i] = !storyElementsFoldout[i];
                                    }
                                    Rect lastRect = GUILayoutUtility.GetLastRect();
                                    lastRect.width = 50f;
                                    lastRect.height = 50f;
                                    lastRect.position = new Vector2(lastRect.position.x + 5f, lastRect.position.y + 3f);

                                    if (UVNFSettings.EditorSettings.ElementHints.ContainsKey(currentElements[i].ElementName))
                                        GUI.DrawTexture(lastRect, UVNFSettings.EditorSettings.ElementHints[currentElements[i].ElementName], ScaleMode.ScaleToFit);
                                }
                                else
                                {
                                    Rect lastRect = GUILayoutUtility.GetLastRect();
                                    lastRect.position = new Vector2(lastRect.position.x, lastRect.position.y - 40f);
                                    if (GUI.Button(lastRect, currentElements[i].ElementName, buttonStyle))
                                    {
                                        storyElementsFoldout[i] = !storyElementsFoldout[i];
                                    }
                                    lastRect.width = 50f;
                                    lastRect.height = 50f;
                                    lastRect.position = new Vector2(lastRect.position.x + 5f, lastRect.position.y + 3f);

                                    if (UVNFSettings.EditorSettings.ElementHints.ContainsKey(currentElements[i].ElementName))
                                        GUI.DrawTexture(lastRect, UVNFSettings.EditorSettings.ElementHints[currentElements[i].ElementName], ScaleMode.ScaleToFit);
                                }

                                if (i == currentElements.Count - 1)
                                    GUILayout.Space(20f);
                            }
                        }

                        if (moveElement != -1)
                        {
                            if (moveUp && moveElement > 0)
                            {
                                SwitchElement(currentElements[moveElement - 1], currentElements[moveElement]);

                                bool shiftFoldout = storyElementsFoldout[moveElement - 1];
                                storyElementsFoldout[moveElement - 1] = storyElementsFoldout[moveElement];
                                storyElementsFoldout[moveElement] = shiftFoldout;

                                storyContainer.ConnectStoryElements();
                            }
                            else if (moveElement < currentElements.Count - 1)
                            {
                                SwitchElement(currentElements[moveElement], currentElements[moveElement + 1]);

                                bool shiftFoldout = storyElementsFoldout[moveElement + 1];
                                storyElementsFoldout[moveElement + 1] = storyElementsFoldout[moveElement];
                                storyElementsFoldout[moveElement] = shiftFoldout;
                            }

                            moveElement = -1;
                        }

                        if (removeElement != -1)
                        {
                            storyContainer.nodes[removeElement].ClearConnections();
                            AssetDatabase.RemoveObjectFromAsset(storyContainer.nodes[removeElement]);
                            AssetDatabase.SaveAssets();
                            storyContainer.nodes.RemoveAt(removeElement);
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

    public void AddElement(Type storyElement)
    {
        StoryElement newElement = CreateInstance(storyElement) as StoryElement;
        newElement.name = newElement.ElementName;

        EditorUtility.SetDirty(newElement);

        AssetDatabase.AddObjectToAsset(newElement, storyContainer);
        storyElementsFoldout.Add(true);

        storyContainer.nodes.Add(newElement);

        StoryElement previousElement = currentElements[currentElements.Count - 1];
        previousElement.GetOutputPort("NextNode").Connect(newElement.GetInputPort("PreviousNode"));

        newElement.position = previousElement.position + new Vector2(330f, 0f);

        AssetDatabase.SaveAssets();
    }

    public void SwitchElement(StoryElement element1, StoryElement element2)
    {
        StoryElement previousStory = element1.GetInputPort("PreviousNode").Connection.node as StoryElement;
        StoryElement nextElement = null;
        if (element2.GetOutputPort("NextNode").IsConnected)
            nextElement = element2.GetOutputPort("NextNode").Connection.node as StoryElement;

        element1.ClearConnections();
        element2.ClearConnections();

        Vector2 firstElementPosition = element1.position;
        element1.position = element2.position;
        element2.position = firstElementPosition;

        if (nextElement != null)
            element1.GetOutputPort("NextNode").Connect(nextElement.GetInputPort("PreviousNode"));
        element2.GetOutputPort("NextNode").Connect(element1.GetInputPort("PreviousNode"));
        element2.GetInputPort("PreviousNode").Connect(previousStory.GetOutputPort("NextNode"));
    }

    private void ChangeBackgroundStyle(Color color)
    {
        Texture2D backgroundTexture = new Texture2D(1, 1);
        backgroundTexture.SetPixel(0, 0, color);
        backgroundTexture.Apply();
        style.normal.background = backgroundTexture;
    }
}
