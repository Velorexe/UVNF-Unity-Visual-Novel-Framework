using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ChangeBackgroundElement : StoryElement
{
    public override string ElementName => "Change Background";

    public override Color32 DisplayColor => UDSFSettings.Settings.SceneryElementColor;

    public override StoryElementTypes Type => StoryElementTypes.Scenery;

    public Sprite NewBackground;
    public bool Fade = true;
    public float FadeTime = 1f;

    public override void DisplayLayout()
    {
        GUILayout.Label("New Background");
        NewBackground = EditorGUILayout.ObjectField(NewBackground, typeof(Sprite), false) as Sprite;

        Fade = GUILayout.Toggle(Fade, "Fade");
        if (Fade)
            FadeTime = EditorGUILayout.FloatField("Fade out time", FadeTime);
    }

    public override IEnumerator Execute(GameManager managerCallback, UDSFCanvas canvas)
    {
        if (Fade)
            return canvas.ChangeBackgroundCoroutine(NewBackground, FadeTime);
        else
            canvas.ChangeBackground(NewBackground);
        return null;
    }
}
