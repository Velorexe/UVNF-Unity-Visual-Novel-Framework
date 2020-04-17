using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class DialogueElement : StoryElement
{
    public override string ElementName => "Dialogue";

    public override Color32 DisplayColor => _displayColor;
    private Color32 _displayColor = new Color32().Story();

    public override StoryElementTypes Type => StoryElementTypes.Story;

    public string CharacterName;
    public string Dialogue;

    private GUIStyle textAreaStyle;

    public override void DisplayLayout(Rect layoutRect)
    {
#if UNITY_EDITOR
        if(textAreaStyle == null)
        {
            textAreaStyle = new GUIStyle("TextArea");
            textAreaStyle.richText = true;
        }

        GUILayout.Label("Character", EditorStyles.boldLabel);
        CharacterName = GUILayout.TextField(CharacterName);
        GUILayout.Label("Dialogue", EditorStyles.boldLabel);
        Dialogue = EditorGUILayout.TextArea(Dialogue, textAreaStyle, GUILayout.ExpandHeight(true), GUILayout.MaxHeight(50));
#endif
    }

    public override IEnumerator Execute(GameManager gameManager, UVNFCanvas canvas)
    {
        return canvas.DisplayText(Dialogue, CharacterName);
    }
}
