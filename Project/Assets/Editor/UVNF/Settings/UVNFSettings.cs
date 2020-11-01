using System.Collections.Generic;
using System.Reflection;
using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UVNF.Core.Story;
using UVNF.Extensions;

namespace UVNF.Editor.Settings
{
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

        private static Dictionary<StoryElementTypes, GUISkin> _elementStyles = new Dictionary<StoryElementTypes, GUISkin>();

        private static UVNFEditorSettings GetEditorSettings()
        {
            if (AssetDatabase.LoadAssetAtPath<UVNFEditorSettings>("Assets/Editor/UVNF/Settings/UVNFEditorSettings.asset") == null)
            {
                AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<UVNFEditorSettings>(), "Assets/Editor/UVNF/Settings/UVNFEditorSettings.asset");
                AssetDatabase.SaveAssets();
            }
            return AssetDatabase.LoadAssetAtPath<UVNFEditorSettings>("Assets/Editor/UVNF/Settings/UVNFEditorSettings.asset");
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
            SetupElements();
            return _elementStyles[type].button;
        }

        public static GUIStyle GetLabelStyle(StoryElementTypes type)
        {
            SetupElements();
            return _elementStyles[type].label;
        }

        public static GUIStyle GetColorByElement(StoryElementTypes type)
        {
            GUIStyle newStyle = EditorSettings.UVNFSkin.button;

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

        private static void SetupElements()
        {
            if(_elementStyles.Count == 0)
            {
                #region Audio
                GUISkin style = GUISkin.Instantiate(EditorSettings.UVNFSkin);
                
                style.button.normal.background = EditorSettings.AudioElementTexture;
                style.label.normal.textColor = new Color32(88, 106, 84, 255);

                _elementStyles.Add(StoryElementTypes.Audio, style);
                #endregion

                #region Character
                style = GUISkin.Instantiate(EditorSettings.UVNFSkin);

                style.button.normal.background = EditorSettings.CharacterElementTexture;
                style.label.normal.textColor = new Color32(63, 58, 51, 255);

                _elementStyles.Add(StoryElementTypes.Character, style);
                #endregion

                #region Scenery
                style = GUISkin.Instantiate(EditorSettings.UVNFSkin);

                style.button.normal.background = EditorSettings.SceneryElementTexture;
                style.label.normal.textColor = new Color32(63, 60, 48, 255);

                _elementStyles.Add(StoryElementTypes.Scenery, style);
                #endregion

                #region Story
                style = GUISkin.Instantiate(EditorSettings.UVNFSkin);

                style.button.normal.background = EditorSettings.StoryElementTexture;
                style.label.normal.textColor = new Color32(63, 48, 48, 255);

                _elementStyles.Add(StoryElementTypes.Story, style);
                #endregion

                #region Utility
                style = GUISkin.Instantiate(EditorSettings.UVNFSkin);

                style.button.normal.background = EditorSettings.UtilityElementTexture;
                style.label.normal.textColor = new Color32(83, 87, 110, 255);

                _elementStyles.Add(StoryElementTypes.Utility, style);
                #endregion

                #region Other
                style = GUISkin.Instantiate(EditorSettings.UVNFSkin);

                style.button.normal.background = EditorSettings.OtherElementTexture;
                style.label.normal.textColor = new Color32(56, 56, 56, 255);

                _elementStyles.Add(StoryElementTypes.Other, style);
                #endregion
            }
        }
    }
}