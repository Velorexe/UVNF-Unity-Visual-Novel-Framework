using System.Collections;
using UnityEngine;
using UVNF.Core.UI;

namespace UVNF.Core.Story.Scenery
{
    /// <summary>
    /// A <see cref="StoryElement"/> that changes the background of the UI
    /// </summary>
    public class ChangeBackgroundElement : StoryElement
    {
        public override string ElementName => "Change Background";

        public override StoryElementTypes Type => StoryElementTypes.Scenery;

        /// <summary>
        /// The new background that should be shown
        /// </summary>
        public Sprite NewBackground;

        /// <summary>
        /// Set to <see langword="true"/> if the background should crossfade with the current background
        /// </summary>
        public bool Fade = true;

        /// <summary>
        /// The time it'll take for the backgrounds to crossfade
        /// </summary>
        public float FadeTime = 1f;

        public override IEnumerator Execute(UVNFManager managerCallback, UVNFCanvas canvas)
        {
            if (Fade)
            {
                canvas.ChangeBackground(NewBackground, FadeTime);
            }
            else
            {
                canvas.ChangeBackground(NewBackground);
            }

            yield return null;
        }
    }
}