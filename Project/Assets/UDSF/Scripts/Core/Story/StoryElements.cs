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
        public override Color32 DisplayColor { get { return new Color32(0xFE, 0xC4, 0xC4, 0xff); } }
        public override StoryElementTypes Type { get { return StoryElementTypes.Story; } }

        public string CharacterName;
        public string Dialogue;

        public override void DisplayLayout()
        {
            GUILayout.Label("Character", EditorStyles.boldLabel);
            CharacterName = GUILayout.TextField(CharacterName);
            GUILayout.Label("Dialogue", EditorStyles.boldLabel);
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
        public override Color32 DisplayColor { get { return new Color32(0xB3, 0xBD, 0xED, 0xff); } }
        public override StoryElementTypes Type { get { return StoryElementTypes.Utility; } }

        public GameObject ObjectToSpawn;

        public override void DisplayLayout()
        {
            GUILayout.Label("Object To Spawn", EditorStyles.boldLabel);
            ObjectToSpawn = EditorGUILayout.ObjectField(ObjectToSpawn, typeof(GameObject), false) as GameObject;
        }

        public override IEnumerable Execute()
        {
            return null;
        }
    }
}
