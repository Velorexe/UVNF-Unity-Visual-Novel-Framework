using System.Collections;
using UnityEngine;
using UVNF.Core.UI;

namespace UVNF.Core.Story.Character
{
    public class ChangeSpriteElement : StoryElement
    {
        public override string ElementName => "Change Sprite";

        public override StoryElementTypes Type => StoryElementTypes.Character;

        public string CharacterName;
        public Sprite NewSprite;

        public override IEnumerator Execute(UVNFManager managerCallback, UVNFCanvas canvas)
        {
            managerCallback.CharacterManager.ChangeCharacterSprite(CharacterName, NewSprite);
            return null;
        }
    }
}