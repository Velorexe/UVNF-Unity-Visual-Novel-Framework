using System.Collections;
using UnityEngine;
using UVNF.Core.UI;

namespace UVNF.Core.Story.Elements.Audio
{
    /// <summary>
    /// A <see cref="StoryElement"/> that pauses the currently playing music
    /// </summary>
    public class PauseMusicElement : StoryElement
    {
        public override string ElementName => "Pause Music";

        public override StoryElementTypes Type => StoryElementTypes.Audio;

        /// <summary>
        /// Set to <see langword="true"/> if the <see cref="AudioManager"/> should crossfade the 
        /// new <see cref="AudioClip"/> with the one that's currently playing
        /// </summary>
        public bool FadeOut = true;

        /// <summary>
        /// Defines the time that the <see cref="AudioManager"/> should take to 
        /// fade out the current music
        /// music clips
        /// </summary>
        [Min(0.1f)]
        public float FadeOutTime = 1f;

        public override IEnumerator Execute(UVNFManager managerCallback, UVNFCanvas canvas)
        {
            if (FadeOut)
            {
                managerCallback.StartCoroutine(managerCallback.AudioManager.PauseMusic(FadeOutTime));
            }
            else
            {
                managerCallback.AudioManager.PauseMusic();
            }

            yield return null;
        }
    }
}
