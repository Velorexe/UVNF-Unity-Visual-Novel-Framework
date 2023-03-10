using System.Collections;
using UVNF.Core.UI;

namespace UVNF.Core.Story.Character
{
    /// <summary>
    /// A <see cref="StoryElement"/> that removes a character from the screen
    /// </summary>
    public class ExitSceneElement : StoryElement
    {
        public override string ElementName => "Exit Scene";

        public override StoryElementTypes Type => StoryElementTypes.Character;

        /// <summary>
        /// The name / key of the character on screen
        /// </summary>
        public string CharacterName;

        /// <summary>
        /// The direction at which the character should exit the screen
        /// </summary>
        public ScenePositions ExitPosition;

        /// <summary>
        /// The time it'll take for the character to exit the screen
        /// </summary>
        public float ExitTime = 2f;

        public override IEnumerator Execute(UVNFManager managerCallback, UVNFCanvas canvas)
        {
            managerCallback.CharacterManager.RemoveCharacter(CharacterName, ExitPosition, ExitTime);
            return null;
        }
    }
}