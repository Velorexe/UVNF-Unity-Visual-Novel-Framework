using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using XNode;
using UVNF.Core.UI;
using UVNF.Extensions;

namespace UVNF.Core.Story.Other
{
    [NodeTint("#CCFCC3"), Serializable]
    public class StartElement : StoryElement
    {
        public override string ElementName => "Start";

        public override Color32 DisplayColor => _displayColor;
        private Color32 _displayColor = new Color32().Other();

        public override StoryElementTypes Type => StoryElementTypes.Other;

        public override bool IsVisible() { return false; }

        public string StoryName;
        public bool IsRoot;

        public override object GetValue(NodePort port)
        {
            if (port.IsConnected)
                return port.Connection.node;
            return null;
        }

        public override IEnumerator Execute(UVNFManager managerCallback, UVNFCanvas canvas) { return null; }

#if UNITY_EDITOR
        public override void DisplayLayout(Rect layoutRect, GUIStyle label) { }

        public override void DisplayNodeLayout(Rect layoutRect)
        {
            GUILayout.BeginHorizontal();
            {
                GUILayout.Label("Story Name:");
                StoryName = EditorGUILayout.TextField(StoryName);
            }
            GUILayout.EndHorizontal();

            IsRoot = GUILayout.Toggle(IsRoot, "Is Root");
        }
    }
#endif
}