using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UDSFSettingsInstance : ScriptableObject
{
    [Header("DBSF GUI Skin")]
    public GUISkin DBSFSkin;

    [Header("Element Textures")]
    public Texture2D StoryElementTexture;
    public Texture2D CharacterElementTexture;
    public Texture2D SceneryElementTexture;
    public Texture2D AudioElementTexture;
    public Texture2D UtilityElementTexture;
    public Texture2D OtherElementTexture;

    [Header("Element Hint Textures")]
    public Texture2D DialogueElementTexture;
}
