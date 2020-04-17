using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class StopAudio : StoryElement
{
    public override string ElementName => "Stop Audio";

    public override Color32 DisplayColor => _displayColor;
    private Color32 _displayColor = new Color32().Audio();

    public override StoryElementTypes Type => StoryElementTypes.Audio;

    public bool PauseBackgroundMusic = true;

    public bool Fade = true;
    public float FadeTime = 1f;

    public override void DisplayLayout(Rect layoutRect)
    {
#if UNITY_EDITOR
        PauseBackgroundMusic = GUILayout.Toggle(PauseBackgroundMusic, "Pause Background Music");
        Fade = GUILayout.Toggle(Fade, "Fade Background Music");
        if (Fade) EditorGUILayout.FloatField("Fade Time", FadeTime);
#endif
    }

    public override IEnumerator Execute(GameManager managerCallback, UVNFCanvas canvas)
    {
        if (PauseBackgroundMusic)
        {
            if (Fade)
                managerCallback.AudioManager.StopBackgroundMusic(FadeTime, false);
            else
                managerCallback.AudioManager.PauseBackgroundMusic();
        }
        else
        {
            if (Fade)
                managerCallback.AudioManager.StopBackgroundMusic(FadeTime);
            else
                managerCallback.AudioManager.StopBackgroundMusic();
        }
        yield return null;
    }
}
