using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AudioDialogueElement : DialogueElement
{
    public override string ElementName => "Audio Dialogue";

    public override StoryElementTypes Type => StoryElementTypes.Story;

    public bool Beep;

    public bool PitchShift;
    public float MaxPitch;

    public AudioClip BoopSoundEffect;
    public AudioClip DialogueClip;

    public override void DisplayLayout(Rect layoutRect)
    {
        base.DisplayLayout(layoutRect);
#if UNITY_EDITOR
        Beep = GUILayout.Toggle(Beep, "Beep");
        if (Beep)
        {
            BoopSoundEffect = EditorGUILayout.ObjectField("Boop Sound Effect", BoopSoundEffect, typeof(AudioClip), false) as AudioClip;
            PitchShift = GUILayout.Toggle(PitchShift, "Pitch Shift");
            MaxPitch = EditorGUILayout.Slider(MaxPitch, 0f, 0.2f);
        }
        else
            DialogueClip = EditorGUILayout.ObjectField("Dialogue", DialogueClip, typeof(AudioClip), false) as AudioClip;
#endif
    }

    public override IEnumerator Execute(GameManager managerCallback, UVNFCanvas canvas)
    {
        if (Beep)
            return canvas.DisplayText(Dialogue, CharacterName, BoopSoundEffect, managerCallback.AudioManager, MaxPitch);
        else
            return canvas.DisplayText(Dialogue, CharacterName, DialogueClip, managerCallback.AudioManager);
    }
}
