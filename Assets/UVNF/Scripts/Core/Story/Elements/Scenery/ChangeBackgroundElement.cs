using System.Collections;
using UnityEngine;
using UVNF.Core.UI;

namespace UVNF.Core.Story.Scenery
{
    public class ChangeBackgroundElement : StoryElement
    {
        public override string ElementName => "Change Background";

        public override StoryElementTypes Type => StoryElementTypes.Scenery;

        public Sprite NewBackground;

        public bool Fade = true;
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