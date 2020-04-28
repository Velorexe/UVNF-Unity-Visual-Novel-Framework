using System.Collections;
using UnityEngine;
using UnityEditor;

public class BackgroundMusicElement : StoryElement
{
    public override string ElementName => "Background Music";

    public override Color32 DisplayColor => _displayColor;
    private Color32 _displayColor = new Color32().Audio();

    public override StoryElementTypes Type => StoryElementTypes.Audio;

    public AudioClip BackgroundMusic;

    public bool Crossfade = true;
    public float CrossfadeTime = 1f;

    public float Volume;

    public override void DisplayLayout(Rect layoutRect)
    {
#if UNITY_EDITOR
        BackgroundMusic = EditorGUILayout.ObjectField(BackgroundMusic, typeof(AudioClip), false) as AudioClip;

        Crossfade = GUILayout.Toggle(Crossfade, "Crossfade");
        if (Crossfade)
            CrossfadeTime = EditorGUILayout.FloatField("Crossfade Time", CrossfadeTime);

        if (CrossfadeTime < 0) CrossfadeTime = 0;
        Volume = EditorGUILayout.Slider("Volume", Volume, 0f, 1f);
#endif
    }

    public override IEnumerator Execute(GameManager managerCallback, UVNFCanvas canvas)
    {
        if (Crossfade)
            managerCallback.AudioManager.CrossfadeBackgroundMusic(BackgroundMusic, CrossfadeTime);
        else
            managerCallback.AudioManager.PlayBackgroundMusic(BackgroundMusic);
        yield return null;
    }
}
