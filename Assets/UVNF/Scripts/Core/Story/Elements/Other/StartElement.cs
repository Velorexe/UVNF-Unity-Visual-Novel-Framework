using System;
using System.Collections;
using UVNF.Core.UI;
using XNode;

namespace UVNF.Core.Story.Other
{
    [NodeTint("#CCFCC3"), Serializable]
    public class StartElement : StoryElement
    {
        public override string ElementName => "Start";

        public override StoryElementTypes Type => StoryElementTypes.Other;

        public override bool IsVisible() { return false; }

        public string StoryName;
        public bool IsRoot;

        public override object GetValue(NodePort port)
        {
            if (port.IsConnected)
                return port.Connection.node;
            return null;
        }

        public override IEnumerator Execute(UVNFManager managerCallback, UVNFCanvas canvas) { return null; }
    }
}