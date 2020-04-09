using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Entities;

public class StoryElements
{
    public class DialogueElement : StoryElement
    {
        public override string ElementName { get { return "Dialogue"; } }
        public override Color32 DisplayColor { get { return new Color32(0x99, 0xe0, 0xac, 0xff); } }

        public string CharacterName;
        public string Dialogue;

        public override void DisplayLayout()
        {
            GUILayout.Label("Character");
            CharacterName = GUILayout.TextField(CharacterName);
            GUILayout.Label("Dialogue");
            Dialogue = EditorGUILayout.TextArea(Dialogue, GUILayout.ExpandHeight(true), GUILayout.MaxHeight(50));
        }

        public override void Execute()
        {

        }
    }
}
