using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System;
using System.Linq;
using UnityEditor;

public static class UDSFSettings
{
    public static UDSFSettingsInstace Settings
    {
        get
        {
            if (_settingsInstance == null)
                _settingsInstance = GetSettings();
            return _settingsInstance;
        }
    }
    private static UDSFSettingsInstace _settingsInstance;

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


    private static UDSFSettingsInstace GetSettings()
    {
        return AssetDatabase.LoadAssetAtPath<UDSFSettingsInstace>("Assets/UDSF/Scripts/Core/Settings/Settings.asset");
    }

    private static void InitializeStoryElements()
    {
        _storyElements = new List<StoryElement>();
        foreach (Type type in
            Assembly.GetAssembly(typeof(StoryElement)).GetTypes()
            .Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(StoryElement))))
        {
            _storyElements.Add((StoryElement)Activator.CreateInstance(type));
        }
        _storyElements.Sort();
    }
}
