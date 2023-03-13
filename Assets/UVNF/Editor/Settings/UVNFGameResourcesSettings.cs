using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using UVNF.Core.UI.Writers;
using UVNF.Editor.Helpers;
using UVNF.Editor.Settings;
using UVNF.Entities;

namespace UVNF.Editor
{
    public static class UVNFGameResourcesSettings
    {
        #region Game Resources

        [SettingsProvider]
        public static SettingsProvider CreateGameResourcesSettingsProvider()
        {
            SettingsProvider provider = new SettingsProvider("Project/UVNF/Game Resources", SettingsScope.Project)
            {
                label = "Game Resources",
                guiHandler = (searchContext) =>
                {
                    if (UVNFEditorSettings.Instance.MainResources == null)
                    {
                        EditorGUILayout.LabelField("Looks like UVNF's main resources file hasn't been created yet, or the reference " +
                            "isn't set in the UVNF Editor Settings (UVNF/Editor/Settings/UVNFEditorSettings). Want to create one now?");

                        if (GUILayout.Button("Create UVNF Game Resources"))
                        {
                            UVNFGameResources gameResources = ScriptableObject.CreateInstance<UVNFGameResources>();

                            gameResources.TextWriterPool = AssemblyHelpers.GetEnumerableOfInterfaceType<ITextWriter>();

                            AssetDatabase.CreateAsset(gameResources, "Assets/UVNFGameResources.asset");

                            AssetDatabase.SaveAssets();
                            AssetDatabase.Refresh();

                            UVNFEditorSettings.Instance.MainResources = gameResources;
                        }
                    }
                    else
                    {
                        EditorGUILayout.LabelField("Looks like you're all set-up and running!");
                    }
                },
                keywords = new HashSet<string>(new[] { "uvnf", "game", "resources", "game resources" })
            };

            return provider;
        }
        #endregion

        #region Characters

        [SettingsProvider]
        public static SettingsProvider CreateCharacterSettingsProvider()
        {
            SettingsProvider provider = new SettingsProvider("Project/UVNF/Characters", SettingsScope.Project)
            {
                label = "Characters",
                guiHandler = (searchContext) =>
                {
                    UVNFGameResources resources = UVNFEditorSettings.Instance.MainResources;
                    {
                        EditorGUILayout.LabelField("lol");
                    }
                },
                keywords = new HashSet<string>(new[] { "uvnf", "characters" })
            };

            return provider;
        }

        #endregion

        #region TextWriters

        private static ReorderableList textWriterList;
        private static UVNFGameResources gameResources;

        [SettingsProvider]
        public static SettingsProvider CreateTextWriterSettingsProvider()
        {
            SettingsProvider provider = new SettingsProvider("Project/UVNF/Text Writers", SettingsScope.Project)
            {
                label = "Text Writers",
                activateHandler = (searchContext, rootElement) =>
                {
                    gameResources = UVNFEditorSettings.Instance.MainResources;

                    textWriterList = new ReorderableList(gameResources.TextWriterPool, typeof(ITextWriter), true, true, false, false)
                    {
                        drawElementCallback = DrawListItems,
                        drawHeaderCallback = DrawHeader
                    };
                },
                guiHandler = (searchContext) =>
                {
                    UVNFGameResources resources = UVNFEditorSettings.Instance.MainResources;
                    {
                        GUILayout.BeginHorizontal();
                        {
                            GUILayout.Space(5f);
                            GUILayout.BeginVertical();
                            {

                                textWriterList.DoLayoutList();

                                EditorGUILayout.LabelField("Default Text Settings", EditorStyles.boldLabel);
                                DrawTextWriterSettings();

                            }
                            GUILayout.EndVertical();
                            GUILayout.Space(5f);
                        }
                        GUILayout.EndHorizontal();
                    }
                },
                keywords = new HashSet<string>(new[] { "uvnf", "textwriters", "text writers", "font" })
            };

            return provider;
        }

        private static void DrawListItems(Rect rect, int index, bool isActive, bool isFocused)
        {
            EditorGUI.LabelField(
                new Rect(rect.x + 20f, rect.y, rect.width, rect.height),
                gameResources.TextWriterPool[index].GetType().Name + (index == 0 ? " (Default)" : ""));
        }

        private static void DrawHeader(Rect rect)
        {
            EditorGUI.LabelField(rect, "Text Writers");
        }

        private static void DrawTextWriterSettings()
        {
            gameResources.DefaultWriterSettings.FontSize = EditorGUILayout.FloatField("Font Size", gameResources.DefaultWriterSettings.FontSize);

            gameResources.DefaultWriterSettings.Font = (TMP_FontAsset)EditorGUILayout.ObjectField(
                "Font",
                gameResources.DefaultWriterSettings.Font,
                typeof(TMP_FontAsset),
                false);

            gameResources.DefaultWriterSettings.Styles = (FontStyles)EditorGUILayout.EnumFlagsField("Styles", gameResources.DefaultWriterSettings.Styles);
            gameResources.DefaultWriterSettings.Color = EditorGUILayout.ColorField("Color", gameResources.DefaultWriterSettings.Color);

            gameResources.DefaultWriterSettings.TextDisplaySpeed = EditorGUILayout.FloatField("Display Speed", gameResources.DefaultWriterSettings.TextDisplaySpeed);
        }
        #endregion
    }
}
