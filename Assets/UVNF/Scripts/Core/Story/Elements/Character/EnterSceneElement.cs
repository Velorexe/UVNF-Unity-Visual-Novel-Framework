using System.Collections;
using UnityEngine;
using UVNF.Core.UI;

namespace UVNF.Core.Story.Character
{
    /// <summary>
    /// A <see cref="StoryElement"/> that lets a character enter the <see cref="CanvasCharacterManager"/> stack
    /// and show it on screen
    /// </summary>
    public class EnterSceneElement : StoryElement
    {
        public override string ElementName => "Enter Scene";

        public override StoryElementTypes Type => StoryElementTypes.Character;

        /// <summary>
        /// The name / key of the character
        /// </summary>
        public string CharacterName;

        /// <summary>
        /// The first sprite that the character should show on screen
        /// </summary>
        public Sprite Character;

        /// <summary>
        /// Set to <see langword="true"/> if the character should have their sprite flipped
        /// </summary>
        public bool Flip = false;

        /// <summary>
        /// The direction from which the character enters the screen
        /// </summary>
        public ScenePositions EnterFromDirection = ScenePositions.Left;

        /// <summary>
        /// The position at which the character will end up on screen
        /// </summary>
        public ScenePositions FinalPosition = ScenePositions.Left;

        /// <summary>
        /// The time it'll take for the character to end up on the <see cref="FinalPosition"/>
        /// </summary>
        public float EnterTime = 2f;

        public override IEnumerator Execute(UVNFManager managerCallback, UVNFCanvas canvas)
        {
            managerCallback.CharacterManager.AddCharacter(CharacterName, Character, Flip, EnterFromDirection, FinalPosition, EnterTime);
            return null;
        }
    }
}