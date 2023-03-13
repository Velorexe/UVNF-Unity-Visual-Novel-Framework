using System.Collections;
using UnityEngine;
using UVNF.Core.UI;

namespace UVNF.Core.Story.Audio
{
    /// <summary>
    /// A <see cref="StoryElement"/> that plays Music through the <see cref="AudioManager"/>
    /// </summary>
    public class MusicElement : StoryElement
    {
        public override string ElementName => "Music";

        public override StoryElementTypes Type => StoryElementTypes.Audio;

        /// <summary>
        /// The music <see cref="AudioClip"/>
        /// </summary>
        public AudioClip Music;

        /// <summary>
        /// Set to <see langword="true"/> if the <see cref="AudioManager"/> should crossfade the 
        /// new <see cref="AudioClip"/> with the one that's currently playing
        /// </summary>
        public bool Crossfade = true;

        /// <summary>
        /// Defines the time that the <see cref="AudioManager"/> should crossfade between
        /// music clips
        /// </summary>
        [Min(0.1f)]
        public float CrossfadeTime = 1f;

        /// <summary>
        /// The volume at which the music should play at
        /// </summary>
        [Range(0f, 1f)]
        public float Volume;

        public override IEnumerator Execute(UVNFManager managerCallback, UVNFCanvas canvas)
        {
            if (Crossfade)
            {
                managerCallback.StartCoroutine(managerCallback.AudioManager.PlayMusic(Music, fadeInTime: CrossfadeTime, volume: Volume));
            }
            else
            {
                managerCallback.AudioManager.PlayMusic(Music, volume: Volume);
            }

            yield return null;
        }
    }
}