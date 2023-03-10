using System.Collections;
using UnityEngine;
using UVNF.Core.UI;

namespace UVNF.Core.Story.Utility
{
    /// <summary>
    /// A <see cref="StoryElement"/> that waits a given time until it resumes with the story
    /// </summary>
    public class WaitElement : StoryElement
    {
        public override string ElementName => "Wait";

        public override StoryElementTypes Type => StoryElementTypes.Utility;

        /// <summary>
        /// The time the story should wait for
        /// </summary>
        [Min(0f)]
        public float WaitTime = 1f;

        public override IEnumerator Execute(UVNFManager managerCallback, UVNFCanvas canvas)
        {
            yield return new WaitForSeconds(WaitTime);
        }
    }
}