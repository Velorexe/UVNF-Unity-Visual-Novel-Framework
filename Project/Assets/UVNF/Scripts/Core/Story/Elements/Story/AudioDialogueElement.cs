using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UVNF.Core.UI;

namespace UVNF.Core.Story.Dialogue
{
    public class AudioDialogueElement : DialogueElement
    {
        public override string ElementName => "Audio Dialogue";

        public override StoryElementTypes Type => StoryElementTypes.Story;

        public bool Beep;
        public bool BeepOnVowel;

        public bool PitchShift;
        public float MaxPitch;

        public AudioClip BoopSoundEffect;
        public AudioClip DialogueClip;

        public override void DisplayLayout(Rect layoutRect, GUIStyle label)
        {
            base.DisplayLayout(layoutRect, label);
#if UNITY_EDITOR
            Beep = GUILayout.Toggle(Beep, "Beep");
            if (Beep)
            {
                BoopSoundEffect = EditorGUILayout.ObjectField("Beep Sound Effect", BoopSoundEffect, typeof(AudioClip), false) as AudioClip;
                BeepOnVowel = GUILayout.Toggle(BeepOnVowel, "Beep Only On Vowel");

                PitchShift = GUILayout.Toggle(PitchShift, "Pitch Shift");
                if (PitchShift)
                    MaxPitch = EditorGUILayout.Slider(MaxPitch, 0f, 0.2f);
            }
            else
                DialogueClip = EditorGUILayout.ObjectField("Dialogue", DialogueClip, typeof(AudioClip), false) as AudioClip;
#endif
        }

        public override IEnumerator Execute(GameManager managerCallback, UVNFCanvas canvas)
        {
            if (Beep)
                return canvas.DisplayText(Dialogue, CharacterName, BoopSoundEffect, managerCallback.AudioManager, MaxPitch, BeepOnVowel);
            else
                return canvas.DisplayText(Dialogue, CharacterName, DialogueClip, managerCallback.AudioManager);
        }
    }
}