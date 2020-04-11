using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UDSFSettingsInstace : ScriptableObject
{
    [Header("Style")]
    public GUIStyle ElementGUIStyle;
    public GUIStyle BoxGUIStyle;

    [Header("Element Type Textures")]
    public Texture2D StoryElementTexture;
    public Texture2D CharacterElementTexture;
    public Texture2D SceneryElementTexture;
    public Texture2D AudioElementTexture;
    public Texture2D UtilityElementTexture;
    public Texture2D OtherElementTexture;

    [Header("Element Textures")]
    public Texture2D DialogueElementTexture;

    public GUIStyle GetElementStyle(StoryElementTypes type)
    {
        GUIStyle newStyle = ElementGUIStyle;
        switch (type)
        {
            case StoryElementTypes.Audio:
                newStyle.normal.background = AudioElementTexture;
                break;
            case StoryElementTypes.Scenery:
                newStyle.normal.background = SceneryElementTexture;
                break;
            case StoryElementTypes.Character:
                newStyle.normal.background = CharacterElementTexture;
                break;
            case StoryElementTypes.Other:
                newStyle.normal.background = OtherElementTexture;
                break;
            case StoryElementTypes.Story:
                newStyle.normal.background = StoryElementTexture;
                break;
            case StoryElementTypes.Utility:
                newStyle.normal.background = UtilityElementTexture;
                break;
        }
        return newStyle;
    }
}
