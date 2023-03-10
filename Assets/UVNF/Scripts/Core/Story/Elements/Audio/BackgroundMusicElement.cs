using System.Collections;
using UnityEngine;
using UVNF.Core.UI;

namespace UVNF.Core.Story.Audio
{
    /// <summary>
    /// A <see cref="StoryElement"/> that plays Background Music through the <see cref="AudioManager"/>
    /// </summary>
    public class BackgroundMusicElement : StoryElement
    {
        public override string ElementName => "Background Music";

        public override StoryElementTypes Type => StoryElementTypes.Audio;

        /// <summary>
        /// The background music <see cref="AudioClip"/>
        /// </summary>
        public AudioClip BackgroundMusic;

        /// <summary>
        /// Set to <see langword="true"/> if the <see cref="AudioManager"/> should crossfade the 
        /// new <see cref="AudioClip"/> with the one that's currently playing
        /// </summary>
        public bool Crossfade = true;

        /// <summary>
        /// Defines the time that the <see cref="AudioManager"/> should crossfade between
        /// music clips
        /// </summary>
        public float CrossfadeTime = 1f;

        /// <summary>
        /// The volume at which the background music should play at
        /// </summary>
        public float Volume;

        public override IEnumerator Execute(UVNFManager managerCallback, UVNFCanvas canvas)
        {
            if (Crossfade)
            {
                managerCallback.AudioManager.CrossfadeBackgroundMusic(BackgroundMusic, CrossfadeTime);
            }
            else
            {
                managerCallback.AudioManager.PlayBackgroundMusic(BackgroundMusic);
            }

            yield return null;
        }
    }
}