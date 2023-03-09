using System.Collections;
using UnityEngine;
using UVNF.Core.UI;

namespace UVNF.Core.Story.Dialogue
{
    public class AudioDialogueElement : DialogueElement
    {
        public override string ElementName => "Audio Dialogue";

        public override StoryElementTypes Type => StoryElementTypes.Story;

        public AudioClip DialogueClip;

        public override IEnumerator Execute(UVNFManager managerCallback, UVNFCanvas canvas)
        {
            return canvas.DisplayText(Dialogue, CharacterName, DialogueClip, managerCallback.AudioManager);
        }
    }
}