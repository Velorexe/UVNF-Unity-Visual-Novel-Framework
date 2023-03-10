using System.Collections;
using UVNF.Core.UI;

namespace UVNF.Core.Story.Character
{
    /// <summary>
    /// A <see cref="StoryElement"/> that moves a character on screen to a given character
    /// </summary>
    public class MoveToCharacterElement : StoryElement
    {
        public override string ElementName => "Move To Character";

        public override StoryElementTypes Type => StoryElementTypes.Character;

        /// <summary>
        /// The name / key of the character on screen
        /// </summary>
        public string CharacterName;

        /// <summary>
        /// The name / key of the character that the <see cref="CharacterName"/> should move to
        /// </summary>
        public string CharacterToMoveTo;

        /// <summary>
        /// The time it'll take for the character to move to the given character
        /// </summary>
        public float MoveTime = 1f;

        public override IEnumerator Execute(UVNFManager managerCallback, UVNFCanvas canvas)
        {
            managerCallback.CharacterManager.MoveCharacterTo(CharacterName, CharacterToMoveTo, MoveTime);
            return null;
        }
    }
}