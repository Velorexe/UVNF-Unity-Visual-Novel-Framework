using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class DialogueElement : StoryElement
{
    public override string ElementName => "Dialogue";
    public override Color32 DisplayColor => new Color32(0xFE, 0xC4, 0xC4, 0xff);
    public override StoryElementTypes Type => StoryElementTypes.Story;

    public string CharacterName;
    public string Dialogue;

    public override void DisplayLayout()
    {
#if UNITY_EDITOR
        GUILayout.Label("Character", EditorStyles.boldLabel);
        CharacterName = GUILayout.TextField(CharacterName);
        GUILayout.Label("Dialogue", EditorStyles.boldLabel);
        Dialogue = EditorGUILayout.TextArea(Dialogue, GUILayout.ExpandHeight(true), GUILayout.MaxHeight(50));
#endif
    }

    public override IEnumerator Execute(GameManager gameManager, UDSFCanvas canvas)
    {
        return canvas.DisplayText(Dialogue, CharacterName);
    }
}
