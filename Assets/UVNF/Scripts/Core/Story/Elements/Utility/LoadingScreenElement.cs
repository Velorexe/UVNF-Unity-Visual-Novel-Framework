using System.Collections;
using UVNF.Core.UI;

namespace UVNF.Core.Story.Utility
{
    public class LoadingScreenElement : StoryElement
    {
        public override string ElementName => "Loading Screen";

        public override StoryElementTypes Type => StoryElementTypes.Utility;

        public bool ShowLoadScreen;

        public float FadeOutTime = 1f;
        public bool FadeOtherElements;

        public bool WaitToFinish = true;

        public override IEnumerator Execute(UVNFManager managerCallback, UVNFCanvas canvas)
        {
            if (WaitToFinish)
            {
                if (ShowLoadScreen)
                {
                    return managerCallback.Canvas.UnfadeCanvasGroup(managerCallback.Canvas.LoadingCanvasGroup, FadeOutTime);
                }
                else
                {
                    return managerCallback.Canvas.FadeCanvasGroup(managerCallback.Canvas.LoadingCanvasGroup, FadeOutTime);
                }
            }
            else
            {
                if (ShowLoadScreen)
                {
                    managerCallback.Canvas.ShowLoadScreen(FadeOutTime, FadeOtherElements);
                }
                else
                {
                    managerCallback.Canvas.HideLoadScreen(FadeOutTime, FadeOtherElements);
                }
            }

            return null;
        }
    }
}