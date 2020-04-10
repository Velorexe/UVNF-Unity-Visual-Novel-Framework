using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEditor;
using Entities;

public class StoryElements
{
    [Serializable]
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

        public override IEnumerable Execute()
        {
            return null;
        }
    }

    [Serializable]
    public class SpawnObjectElement : StoryElement
    {
        public override string ElementName { get { return "Spawn Object"; } }
        public override Color32 DisplayColor { get { return new Color32(0x99, 0xbd, 0xf7, 0xff); } }

        public GameObject ObjectToSpawn;

        public override void DisplayLayout()
        {
            GUILayout.Label("Object To Spawn");
            ObjectToSpawn = EditorGUILayout.ObjectField(ObjectToSpawn, typeof(GameObject), false) as GameObject;
        }

        public override IEnumerable Execute()
        {
            return null;
        }
    }
}
