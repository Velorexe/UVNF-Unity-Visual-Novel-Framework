using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

public abstract class EditorWindowExtended : EditorWindow
{
    public static int SelectedIndex = 0;

    static Vector2 _listScrollPosition = new Vector2();
    static GUIStyle selectedStyle;
    static GUIStyle boldFoldoutStyle;

    #region convenience UI layout shortcuts
    protected void BoxVertical(params GUILayoutOption[] options)
    {
        GUILayout.BeginVertical("box", options);
    }
    protected void EndVertical()
    {
        GUILayout.EndVertical();
    }
    protected void BoxHorizontal(params GUILayoutOption[] options)
    {
        GUILayout.BeginHorizontal("box", options);
    }
    protected void EndHorizontal()
    {
        GUILayout.EndHorizontal();
    }
    protected void BeginHorizontal(params GUILayoutOption[] options)
    {
        GUILayout.BeginHorizontal(options);
    }
    protected void BeginVertical(params GUILayoutOption[] options)
    {
        GUILayout.BeginVertical(options);
    }
    protected void Space(float pixels)
    {
        GUILayout.Space(pixels);
    }
    protected GUILayoutOption Width(float width)
    {
        return GUILayout.Width(width);
    }
    protected GUILayoutOption MaxWidth(float maxWidth)
    {
        return GUILayout.MaxWidth(maxWidth);
    }
    protected GUILayoutOption Height(float height)
    {
        return GUILayout.Height(height);
    }
    protected GUILayoutOption MaxHeight(float maxHeight)
    {
        return GUILayout.MaxHeight(maxHeight);
    }
    protected void Label(string label, params GUILayoutOption[] options)
    {
        EditorGUILayout.LabelField(label, options);
    }
    #endregion

    protected void DrawItemList(string listName, object[] items, params GUILayoutOption[] options)
    {
        BoxVertical(options);
        {
            GUILayout.Label(listName, EditorStyles.boldLabel);
            BoxVertical(options);
            {
                _listScrollPosition = GUILayout.BeginScrollView(_listScrollPosition, false, true, options);
                {
                    for (int i = 0; i < items.Length; i++)
                    {
                        GUIStyle style = i == SelectedIndex ? SelectedStyle() : GUIStyle.none;
                        BoxHorizontal();
                        {
                            if (GUILayout.Button(items[i].ToString(), style, Width(156f)))
                            {
                                SelectedIndex = i;
                            }
                        }
                        EndHorizontal();
                        Space(-5);
                    }
                }
                GUILayout.EndScrollView();
            }
            EndVertical();
        }
    }

    protected static GUIStyle SelectedStyle()
    {
        if (selectedStyle == null)
        {
            selectedStyle = new GUIStyle();
            var grayTexture = new Texture2D(1, 1);
            var whiteTexture = new Texture2D(1, 1);
            grayTexture.SetPixel(0, 0, Color.gray);
            whiteTexture.SetPixel(0, 0, Color.white);
            selectedStyle.normal.background = whiteTexture;
            selectedStyle.active.background = grayTexture;
        }
        return selectedStyle;
    }
    protected static GUIStyle BoldFoldoutStyle()
    {
        if (boldFoldoutStyle == null)
        {
            boldFoldoutStyle = EditorStyles.foldout;
            boldFoldoutStyle.fontStyle = FontStyle.Bold;
            boldFoldoutStyle.active.textColor = boldFoldoutStyle.onNormal.textColor;
            boldFoldoutStyle.focused.textColor = boldFoldoutStyle.onNormal.textColor;
            boldFoldoutStyle.hover.textColor = boldFoldoutStyle.onNormal.textColor;
        }
        return boldFoldoutStyle;
    }
}
