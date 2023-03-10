using System.Collections;
using UnityEngine;
using UVNF.Core.UI;

namespace UVNF.Core.Story.Character
{
    /// <summary>
    /// A <see cref="StoryElement"/> that changes the sprite of a <see cref="Entities.Character"/> on screen
    /// </summary>
    public class ChangeSpriteElement : StoryElement
    {
        public override string ElementName => "Change Sprite";

        public override StoryElementTypes Type => StoryElementTypes.Character;

        /// <summary>
        /// The name / key of the character on screen
        /// </summary>
        public string CharacterName;

        /// <summary>
        /// The new sprite that the character should display
        /// </summary>
        public Sprite NewSprite;

        public override IEnumerator Execute(UVNFManager managerCallback, UVNFCanvas canvas)
        {
            managerCallback.CharacterManager.ChangeCharacterSprite(CharacterName, NewSprite);
            return null;
        }
    }
}