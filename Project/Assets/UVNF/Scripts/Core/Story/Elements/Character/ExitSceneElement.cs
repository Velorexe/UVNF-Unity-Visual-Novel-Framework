using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ExitSceneElement : StoryElement
{
    public override string ElementName => "Exit Scene";

    public override Color32 DisplayColor => _displayColor;
    private Color32 _displayColor = new Color32().Character();

    public override StoryElementTypes Type => StoryElementTypes.Character;

    public string CharacterName;
    public ScenePositions ExitPosition;
    public float ExitTime;

    public override void DisplayLayout(Rect layoutRect)
    {
#if UNITY_EDITOR
        CharacterName = EditorGUILayout.TextField("Character Name", CharacterName);
        ExitPosition = (ScenePositions)EditorGUILayout.EnumPopup("Exit Position", ExitPosition);
        ExitTime = EditorGUILayout.Slider("Exit Time", ExitTime, 1f, 10f);
#endif
    }

    public override IEnumerator Execute(GameManager managerCallback, UVNFCanvas canvas)
    {
        managerCallback.CharacterManager.RemoveCharacter(CharacterName, ExitPosition, ExitTime);
        return null;
    }
}
