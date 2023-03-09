using System.Collections;
using UnityEngine;
using UVNF.Core.UI;

namespace UVNF.Core.Story.Audio
{
    public class BackgroundMusicElement : StoryElement
    {
        public override string ElementName => "Background Music";

        public override StoryElementTypes Type => StoryElementTypes.Audio;

        public AudioClip BackgroundMusic;

        public bool Crossfade = true;
        public float CrossfadeTime = 1f;

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