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

        /// <summary>
        /// The volume at which the <see cref="DialogueClip"/> should play at
        /// </summary>
        [Range(0f, 1f)]
        public float DialogueVolume;

        public override IEnumerator Execute(UVNFManager managerCallback, UVNFCanvas canvas)
        {
            return canvas.DisplayText(Dialogue, CharacterName, DialogueClip, DialogueVolume, managerCallback.AudioManager);
        }
    }
}