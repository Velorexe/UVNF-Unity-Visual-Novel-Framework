using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UVNF.Core.UI;
using UVNF.Extensions;

namespace UVNF.Core.Story.Character
{
    public class MoveToCharacterElement : StoryElement
    {
        public override string ElementName => "Move To Character";

        public override Color32 DisplayColor => _displayColor;
        private Color32 _displayColor = new Color32().Character();

        public override StoryElementTypes Type => StoryElementTypes.Character;

        public string Character;
        public string CharacterToMoveTo;

        public float MoveTime = 1f;

        public override void DisplayLayout(Rect layoutRect, GUIStyle label)
        {
#if UNITY_EDITOR
            Character = EditorGUILayout.TextField("Character", Character);
            CharacterToMoveTo = EditorGUILayout.TextField("Character To Move To", CharacterToMoveTo);

            MoveTime = EditorGUILayout.FloatField("Move Time", MoveTime);
#endif
        }

        public override IEnumerator Execute(GameManager managerCallback, UVNFCanvas canvas)
        {
            managerCallback.CharacterManager.MoveCharacterTo(Character, CharacterToMoveTo, MoveTime);
            return null;
        }
    }
}