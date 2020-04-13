using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UDSFSettingsInstance : ScriptableObject, ISerializationCallbackReceiver
{
    [Header("DBSF GUI Skin")]
    public GUISkin DBSFSkin;
    public Color32 ActiveElementColor = new Color32(0xf1, 0xd1, 0xff, 0xff);

    [Header("Element Textures")]
    public Texture2D StoryElementTexture;
    public Texture2D CharacterElementTexture;
    public Texture2D SceneryElementTexture;
    public Texture2D AudioElementTexture;
    public Texture2D UtilityElementTexture;
    public Texture2D OtherElementTexture;

    [Header("Element Colors")]
    public Color StoryElementColor = new Color32(0xFE, 0xC4, 0xC4, 0xff);
    public Color CharacterElementColor = new Color32(0xFE, 0xEC, 0xCE, 0xff);
    public Color SceneryElementColor = new Color32(0xFF, 0xF0, 0xAA, 0xff);
    public Color AudioElementColor = new Color32(0xCC, 0xFC, 0xC3, 0xff);
    public Color UtilityElementColor = new Color32(0xB3, 0xBD, 0xED, 0xff);
    public Color OtherElementColor = new Color32(0xB7, 0xB7, 0xB7, 0xff);

    [Header("Element Hint Textures")]
    public List<string> ElementHintTexturesName = new List<string>();
    public List<Texture2D> ElementHintTextures = new List<Texture2D>();

    public Dictionary<string, Texture2D> ElementHints = new Dictionary<string, Texture2D>();

    public void OnBeforeSerialize()
    {
        
    }

    public void OnAfterDeserialize()
    {
        ElementHints = ElementHintTexturesName.Zip(ElementHintTextures, (k, v) => new { k, v }).ToDictionary(x => x.k, x => x.v);
    }

}
