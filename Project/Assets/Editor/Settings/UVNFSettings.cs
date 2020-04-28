using System.Collections.Generic;
using System.Reflection;
using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

public static class UVNFSettings
{
    public static UVNFEditorSettings EditorSettings
    {
        get
        {
            if (_editorSettingsInstance == null)
                _editorSettingsInstance = GetEditorSettings();
            return _editorSettingsInstance;
        }
    }
    private static UVNFEditorSettings _editorSettingsInstance;

    public static List<StoryElement> StoryElements 
    {
        get 
        {
            if (_storyElements == null)
                InitializeStoryElements();
            return _storyElements; 
        }
    }
    public static string[] StoryElementNames
    {
        get 
        {
            if (_storyElements == null)
                InitializeStoryElements();
            return _storyElements.Select(x => x.ElementName).ToArray(); 
        }
    }
    private static List<StoryElement> _storyElements;


    private static UVNFEditorSettings GetEditorSettings()
    {
        if(AssetDatabase.LoadAssetAtPath<UVNFEditorSettings>("Assets/Editor/Settings/UVNFEditorSettings.asset") == null)
        {
            AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<UVNFEditorSettings>(), "Assets/Editor/Settings/UVNFEditorSettings.asset");
            AssetDatabase.SaveAssets();
        }
        return AssetDatabase.LoadAssetAtPath<UVNFEditorSettings>("Assets/Editor/Settings/UVNFEditorSettings.asset");
    }

    private static void InitializeStoryElements()
    {
        _storyElements = new List<StoryElement>();
        foreach (Type type in
            Assembly.GetAssembly(typeof(StoryElement)).GetTypes()
            .Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(StoryElement))))
        {
            _storyElements.Add(ScriptableObject.CreateInstance(type) as StoryElement);
        }
        _storyElements.Sort();
    }

    public static GUIStyle GetElementStyle(StoryElementTypes type)
    {
        GUIStyle newStyle = EditorSettings.DVNFSkin.button;
        switch (type)
        {
            case StoryElementTypes.Audio:
                newStyle.normal.background = EditorSettings.AudioElementTexture;
                break;
            case StoryElementTypes.Scenery:
                newStyle.normal.background = EditorSettings.SceneryElementTexture;
                break;
            case StoryElementTypes.Character:
                newStyle.normal.background = EditorSettings.CharacterElementTexture;
                break;
            case StoryElementTypes.Other:
                newStyle.normal.background = EditorSettings.OtherElementTexture;
                break;
            case StoryElementTypes.Story:
                newStyle.normal.background = EditorSettings.StoryElementTexture;
                break;
            case StoryElementTypes.Utility:
                newStyle.normal.background = EditorSettings.UtilityElementTexture;
                break;
        }
        return newStyle;
    }

    public static GUIStyle GetColorByElement(StoryElementTypes type)
    {
        GUIStyle newStyle = EditorSettings.DVNFSkin.button;

        Texture2D background = new Texture2D(1, 1);
        newStyle.normal.background = background;

        switch (type)
        {
            case StoryElementTypes.Audio:
                newStyle.normal.background.SetPixel(0, 0, new Color32().Audio());
                break;
            case StoryElementTypes.Character:
                newStyle.normal.background.SetPixel(0, 0, new Color32().Character());
                break;
            case StoryElementTypes.Scenery:
                newStyle.normal.background.SetPixel(0, 0, new Color32().Scene());
                break;
            case StoryElementTypes.Story:
                newStyle.normal.background.SetPixel(0, 0, new Color32().Story());
                break;
            case StoryElementTypes.Utility:
                newStyle.normal.background.SetPixel(0, 0, new Color32().Utility());
                break;
            case StoryElementTypes.Other:
                newStyle.normal.background.SetPixel(0, 0, new Color32().Other());
                break;
        }

        newStyle.normal.background.Apply();
        return newStyle;
    }
}
