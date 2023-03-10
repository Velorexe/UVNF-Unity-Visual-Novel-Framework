using System.Collections;
using UnityEngine;
using UVNF.Core.UI;

namespace UVNF.Core.Story.Dialogue
{
    /// <summary>
    /// A <see cref="StoryElement"/> that displays dialogue on the UI
    /// </summary>
    public class DialogueElement : StoryElement
    {
        public override string ElementName => "Dialogue";

        public override StoryElementTypes Type => StoryElementTypes.Story;

        /// <summary>
        /// The text that should appear in the dialogue's name panel
        /// </summary>
        public string CharacterName;

        /// <summary>
        /// The dialogue that should be displayed on the UI
        /// </summary>
        [TextArea(8, 10)]
        public string Dialogue;

        public override IEnumerator Execute(UVNFManager gameManager, UVNFCanvas canvas)
        {
            return canvas.DisplayText(Dialogue, CharacterName);
        }
    }
}