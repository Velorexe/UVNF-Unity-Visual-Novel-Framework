using System.Collections;
using UnityEngine;
using UVNF.Core.UI;

namespace UVNF.Core.Story.Utility
{
    public class WaitElement : StoryElement
    {
        public override string ElementName => "Wait";

        public override StoryElementTypes Type => StoryElementTypes.Utility;

        [Min(0f)]
        public float WaitTime = 1f;

        public override IEnumerator Execute(UVNFManager managerCallback, UVNFCanvas canvas)
        {
            yield return new WaitForSeconds(WaitTime);
        }
    }
}