using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

public static class UDSFSettings
{
    public static UDSFSettingsInstance Settings
    {
        get
        {
            if (_settingsInstance == null)
                _settingsInstance = GetSettings();
            return _settingsInstance;
        }
    }
    private static UDSFSettingsInstance _settingsInstance;

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


    private static UDSFSettingsInstance GetSettings()
    {
        if(AssetDatabase.LoadAssetAtPath<UDSFSettingsInstance>("Assets/Editor/Settings/UDSFSettingsInstance.asset") == null)
        {
            AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<UDSFSettingsInstance>(), "Assets/Editor/Settings/UDSFSettingsInstance.asset");
            AssetDatabase.SaveAssets();
        }
        return AssetDatabase.LoadAssetAtPath<UDSFSettingsInstance>("Assets/Editor/Settings/UDSFSettingsInstance.asset");
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
        GUIStyle newStyle = Settings.DBSFSkin.button;
        switch (type)
        {
            case StoryElementTypes.Audio:
                newStyle.normal.background = Settings.AudioElementTexture;
                break;
            case StoryElementTypes.Scenery:
                newStyle.normal.background = Settings.SceneryElementTexture;
                break;
            case StoryElementTypes.Character:
                newStyle.normal.background = Settings.CharacterElementTexture;
                break;
            case StoryElementTypes.Other:
                newStyle.normal.background = Settings.OtherElementTexture;
                break;
            case StoryElementTypes.Story:
                newStyle.normal.background = Settings.StoryElementTexture;
                break;
            case StoryElementTypes.Utility:
                newStyle.normal.background = Settings.UtilityElementTexture;
                break;
        }
        return newStyle;
    }
}
