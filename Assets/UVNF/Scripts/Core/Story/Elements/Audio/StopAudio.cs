using System.Collections;
using UVNF.Core.UI;

namespace UVNF.Core.Story.Audio
{
    public class StopAudio : StoryElement
    {
        public override string ElementName => "Stop Audio";

        public override StoryElementTypes Type => StoryElementTypes.Audio;

        public bool PauseBackgroundMusic = true;

        public bool Fade = true;
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