using System.Collections;
using UVNF.Core.UI;

namespace UVNF.Core.Story.Audio
{
    /// <summary>
    /// A <see cref="StoryElement"/> that stops certain audio channels
    /// </summary>
    public class StopAudio : StoryElement
    {
        public override string ElementName => "Stop Audio";

        public override StoryElementTypes Type => StoryElementTypes.Audio;

        /// <summary>
        /// Set to <see langword="false"/> if it should stop the background music (reset to start)
        /// </summary>
        public bool PauseBackgroundMusic = true;

        /// <summary>
        /// Set to <see langword="true"/> if the audio should fade, rather than stop / pauze immediately
        /// </summary>
        public bool Fade = true;

        /// <summary>
        /// Defines the time that the <see cref="AudioManager"/> fade for
        /// </summary>
        public float FadeTime = 1f;

        public override IEnumerator Execute(UVNFManager managerCallback, UVNFCanvas canvas)
        {
            if (PauseBackgroundMusic)
            {
                if (Fade)
                {
                    managerCallback.AudioManager.StopBackgroundMusic(FadeTime, false);
                }
                else
                {
                    managerCallback.AudioManager.PauseBackgroundMusic();
                }
            }
            else
            {
                if (Fade)
                {
                    managerCallback.AudioManager.StopBackgroundMusic(FadeTime);
                }
                else
                {
                    managerCallback.AudioManager.StopBackgroundMusic();
                }
            }

            yield return null;
        }
    }
}