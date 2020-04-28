using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class LogElement : StoryElement
{
    public override string ElementName => "Log";

    public override Color32 DisplayColor => _displayColor;
    private Color32 _displayColor = new Color32().Other();

    public override StoryElementTypes Type => StoryElementTypes.Other;

    public string LogText;

    public override void DisplayLayout(Rect layoutRect)
    {
#if UNITY_EDITOR
        LogText = EditorGUILayout.TextField("Log Text", LogText);
#endif
    }

    public override IEnumerator Execute(GameManager managerCallback, UVNFCanvas canvas)
    {
        Debug.Log(LogText);
        return null;
    }
}
