using System.Collections;
using UVNF.Core.UI;

namespace UVNF.Core.Story.Character
{
    public class ExitSceneElement : StoryElement
    {
        public override string ElementName => "Exit Scene";

        public override StoryElementTypes Type => StoryElementTypes.Character;

        public string CharacterName;
        public ScenePositions ExitPosition;

        public float ExitTime;

        public override IEnumerator Execute(UVNFManager managerCallback, UVNFCanvas canvas)
        {
            managerCallback.CharacterManager.RemoveCharacter(CharacterName, ExitPosition, ExitTime);
            return null;
        }
    }
}