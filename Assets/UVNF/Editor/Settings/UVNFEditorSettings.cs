﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UVNF.Editor.Helpers;
using UVNF.Entities;
using UVNF.Entities.Containers.Variables;
using UVNF.Extensions;

namespace UVNF.Editor.Settings
{
    public class UVNFEditorSettings : ScriptableObject, ISerializationCallbackReceiver
    {
        public static UVNFEditorSettings Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = AssemblyHelpers.FindAssetsByType<UVNFEditorSettings>()[0];
                }

                return _instance;
            }
        }
        private static UVNFEditorSettings _instance;

        [Header("DVNF GUI Skin")]
        public GUISkin UVNFSkin;
        public Color32 ActiveElementColor = new Color32(0xf1, 0xd1, 0xff, 0xff);

        [Header("Element Textures")]
        public Texture2D StoryElementTexture;
        public Texture2D CharacterElementTexture;
        public Texture2D SceneryElementTexture;
        public Texture2D AudioElementTexture;
        public Texture2D UtilityElementTexture;
        public Texture2D OtherElementTexture;

        [Header("Element Colors")]
        public Color StoryElementColor = new Color32().Story();
        public Color CharacterElementColor = new Color32().Character();
        public Color SceneryElementColor = new Color32().Scene();
        public Color AudioElementColor = new Color32().Audio();
        public Color UtilityElementColor = new Color32().Utility();
        public Color OtherElementColor = new Color32().Other();

        [Header("Element Hint Textures")]
        public List<string> ElementHintTexturesName = new List<string>();
        public List<Texture2D> ElementHintTextures = new List<Texture2D>();

        public Dictionary<string, Texture2D> ElementHints = new Dictionary<string, Texture2D>();

        [Header("Main Game Resources")]
        public UVNFGameResources MainResources;

        [Header("Variables")]
        public VariableManager Variables;

        public void OnBeforeSerialize()
        {

        }

        public void OnAfterDeserialize()
        {
            ElementHints = ElementHintTexturesName.Zip(ElementHintTextures, (k, v) => new { k, v }).ToDictionary(x => x.k, x => x.v);
        }
    }
}
