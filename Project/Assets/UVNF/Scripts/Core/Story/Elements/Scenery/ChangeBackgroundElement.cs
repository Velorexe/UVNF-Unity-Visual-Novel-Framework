using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ChangeBackgroundElement : StoryElement
{
    public override string ElementName => "Change Background";

    public override Color32 DisplayColor => _displayColor;
    private Color32 _displayColor = new Color32().Scene();

    public override StoryElementTypes Type => StoryElementTypes.Scenery;

    public Sprite NewBackground;
    public bool Fade = true;
    public float FadeTime = 1f;

    public override void DisplayLayout(Rect layoutRect)
    {
#if UNITY_EDITOR
        GUILayout.Label("New Background");
        NewBackground = EditorGUILayout.ObjectField(NewBackground, typeof(Sprite), false) as Sprite;

        Fade = GUILayout.Toggle(Fade, "Fade");
        if (Fade)
            FadeTime = EditorGUILayout.FloatField("Fade out time", FadeTime);
#endif
    }

    public override IEnumerator Execute(GameManager managerCallback, UVNFCanvas canvas)
    {
        if (Fade)
            canvas.ChangeBackground(NewBackground, FadeTime);
        else
            canvas.ChangeBackground(NewBackground);
        return null;
    }
}
