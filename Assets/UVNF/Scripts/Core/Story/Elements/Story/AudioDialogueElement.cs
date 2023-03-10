using System.Collections;
using UnityEngine;
using UVNF.Core.UI;

namespace UVNF.Core.Story.Dialogue
{
    /// <summary>
    /// A <see cref="StoryElement"/> that plays a <see cref="AudioClip"/> over the text dialogue
    /// </summary>
    public class AudioDialogueElement : DialogueElement
    {
        public override string ElementName => "Audio Dialogue";

        public override StoryElementTypes Type => StoryElementTypes.Story;

        /// <summary>
        /// The <see cref="AudioClip"/> that should play during the dialogue
        /// </summary>
        public AudioClip DialogueClip;

        public override IEnumerator Execute(UVNFManager managerCallback, UVNFCanvas canvas)
        {
            return canvas.DisplayText(Dialogue, CharacterName, DialogueClip, managerCallback.AudioManager);
        }
    }
}