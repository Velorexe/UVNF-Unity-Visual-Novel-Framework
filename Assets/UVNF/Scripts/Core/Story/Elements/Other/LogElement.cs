using System.Collections;
using UnityEngine;
using UVNF.Core.UI;

namespace UVNF.Core.Story.Other
{
    public class LogElement : StoryElement
    {
        public override string ElementName => "Log";

        public override StoryElementTypes Type => StoryElementTypes.Other;

        public string LogText;

        public override IEnumerator Execute(UVNFManager managerCallback, UVNFCanvas canvas)
        {
            Debug.Log(LogText);
            return null;
        }
    }
}