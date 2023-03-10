using System.Collections;
using UnityEngine;
using UVNF.Core.UI;

namespace UVNF.Core.Story.Other
{
    /// <summary>
    /// [DEBUG] A <see cref="StoryElement"/> that logs to Unity's <see cref="Debug"/> system
    /// </summary>
    public class LogElement : StoryElement
    {
        public override string ElementName => "Log";

        public override StoryElementTypes Type => StoryElementTypes.Other;

        /// <summary>
        /// The text that should be logged
        /// </summary>
        public string LogText;

        public override IEnumerator Execute(UVNFManager managerCallback, UVNFCanvas canvas)
        {
            Debug.Log(LogText);
            return null;
        }
    }
}