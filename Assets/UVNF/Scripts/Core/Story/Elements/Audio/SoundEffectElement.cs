using CoroutineManager;
using System.Collections;
using UnityEngine;
using UVNF.Core.UI;

namespace UVNF.Core.Story.Audio
{
    public class SoundEffectElement : StoryElement
    {
        public override string ElementName => "Sound Effect";

        public override StoryElementTypes Type => StoryElementTypes.Audio;

        public AudioClip AudioClip;
        public float Volume = 0.5f;

        public bool WaitForAudio = false;

        public override IEnumerator Execute(UVNFManager managerCallback, UVNFCanvas canvas)
        {
            if (WaitForAudio)
            {
                Task task = new Task(managerCallback.AudioManager.PlaySoundCoroutine(AudioClip, Volume), true);
                while (task.Running) yield return null;
            }
            else
            {
                managerCallback.AudioManager.PlaySound(AudioClip, Volume);
            }
        }
    }
}