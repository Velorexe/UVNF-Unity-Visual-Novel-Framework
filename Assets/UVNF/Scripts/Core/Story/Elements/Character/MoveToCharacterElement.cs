using System.Collections;
using UVNF.Core.UI;

namespace UVNF.Core.Story.Character
{
    public class MoveToCharacterElement : StoryElement
    {
        public override string ElementName => "Move To Character";

        public override StoryElementTypes Type => StoryElementTypes.Character;

        public string Character;
        public string CharacterToMoveTo;

        public float MoveTime = 1f;

        public override IEnumerator Execute(UVNFManager managerCallback, UVNFCanvas canvas)
        {
            managerCallback.CharacterManager.MoveCharacterTo(Character, CharacterToMoveTo, MoveTime);
            return null;
        }
    }
}