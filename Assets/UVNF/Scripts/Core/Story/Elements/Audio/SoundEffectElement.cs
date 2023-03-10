using CoroutineManager;
using System.Collections;
using UnityEngine;
using UVNF.Core.UI;

namespace UVNF.Core.Story.Audio
{
    /// <summary>
    /// A <see cref="StoryElement"/> that plays a single <see cref="UnityEngine.AudioClip"/> as a sound effect
    /// </summary>
    public class SoundEffectElement : StoryElement
    {
        public override string ElementName => "Sound Effect";

        public override StoryElementTypes Type => StoryElementTypes.Audio;

        /// <summary>
        /// The <see cref="AudioClip"/> that should play as a sound effect
        /// </summary>
        public AudioClip AudioClip;

        /// <summary>
        /// The volume at which the sound effect should be played
        /// </summary>
        public float Volume = 0.5f;

        /// <summary>
        /// Set to <see langword="true"/> if the story should wait until the sound effect has finished playing
        /// </summary>
        public bool WaitForAudio = false;

        public override IEnumerator Execute(UVNFManager managerCallback, UVNFCanvas canvas)
        {
            if (WaitForAudio)
            {
                Task task = new Task(managerCallback.AudioManager.PlaySoundCoroutine(AudioClip, Volume), true);
                while (task.Running)
                {
                    yield return null;
                }
            }
            else
            {
                managerCallback.AudioManager.PlaySound(AudioClip, Volume);
            }
        }
    }
}