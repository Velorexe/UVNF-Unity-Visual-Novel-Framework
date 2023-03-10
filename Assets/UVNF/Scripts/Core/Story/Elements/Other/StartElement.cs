using System;
using System.Collections;
using UVNF.Core.UI;

namespace UVNF.Core.Story.Other
{
    /// <summary>
    /// A <see cref="StoryElement"/> that defines the starting / entrypoint of the StoryGraph
    /// </summary>
    [NodeTint("#CCFCC3"), Serializable]
    public class StartElement : StoryElement
    {
        public override string ElementName => "Start";

        public override StoryElementTypes Type => StoryElementTypes.Other;

        // Should not be visible, since only 1 should be inside a StoryGraph at any point
        public override bool IsVisible() { return false; }

        /// <summary>
        /// The name of the Story
        /// </summary>
        public string StoryName;

        /// <summary>
        /// Set to <see langword="true"/> if this StoryGraph is the first graph that's executed in the game
        /// </summary>
        public bool IsRoot;

        public override IEnumerator Execute(UVNFManager managerCallback, UVNFCanvas canvas) { return null; }
    }
}