using System.Collections;
using UnityEngine;
using UVNF.Core.UI;

namespace UVNF.Core.Story.Character
{
    public class EnterSceneElement : StoryElement
    {
        public override string ElementName => "Enter Scene";

        public override StoryElementTypes Type => StoryElementTypes.Character;

        public string CharacterName;

        public Sprite Character;

        public bool Flip = false;

        public ScenePositions EnterFromDirection = ScenePositions.Left;
        public ScenePositions FinalPosition = ScenePositions.Left;

        public float EnterTime = 2f;

        public override IEnumerator Execute(UVNFManager managerCallback, UVNFCanvas canvas)
        {
            managerCallback.CharacterManager.AddCharacter(CharacterName, Character, Flip, EnterFromDirection, FinalPosition, EnterTime);
            return null;
        }
    }
}