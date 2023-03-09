using System.Collections;
using UnityEngine;
using UVNF.Core.UI;

namespace UVNF.Core.Story.Dialogue
{
    public class DialogueElement : StoryElement
    {
        public override string ElementName => "Dialogue";

        public override StoryElementTypes Type => StoryElementTypes.Story;

        public string CharacterName;

        [TextArea(8, 10)]
        public string Dialogue;

        public override IEnumerator Execute(UVNFManager gameManager, UVNFCanvas canvas)
        {
            return canvas.DisplayText(Dialogue, CharacterName);
        }
    }
}