using System.Collections;
using UnityEditor;
using UnityEngine;
using UVNF.Core.UI;
using UVNF.Extensions;

namespace UVNF.Core.Story.Character
{
    public class EnterSceneElement : StoryElement
    {
        public override string ElementName => "Enter Scene";

        public override Color32 DisplayColor => _displayColor;
        private Color32 _displayColor = new Color32().Character();

        public override StoryElementTypes Type => StoryElementTypes.Character;

        public string CharacterName;

        public Sprite Character;
        private bool foldOut = false;

        public bool Flip = false;

        public ScenePositions EnterFromDirection = ScenePositions.Left;
        public ScenePositions FinalPosition = ScenePositions.Left;

        public float EnterTime = 2f;

        public override void OnCreate()
        {
            base.OnCreate();
        }

        public override void DisplayLayout(Rect layoutRect, GUIStyle label)
        {
#if UNITY_EDITOR
            CharacterName = EditorGUILayout.TextField("Character Name", CharacterName);

            GUILayout.BeginHorizontal();
            {
                GUILayout.Label("Character Sprite", GUILayout.MaxWidth(147));
                Character = EditorGUILayout.ObjectField(Character, typeof(Sprite), false) as Sprite;
            }
            GUILayout.EndHorizontal();

            Flip = GUILayout.Toggle(Flip, "Flip");

            if (Character != null)
            {
                foldOut = EditorGUILayout.Foldout(foldOut, "Preview", true);
                if (foldOut)
                {
                    layoutRect.position = new Vector2(layoutRect.x, layoutRect.y + 70);
                    layoutRect.width = 1000;
                    layoutRect.height = 500;

                    layoutRect.width = Character.rect.width / (Character.rect.height / 500);
                    //if (Flip) layoutRect.width = -layoutRect.width * 2;
                    layoutRect.height = 500;

                    GUI.DrawTexture(layoutRect, Character.texture, ScaleMode.ScaleToFit);
                    GUILayout.Space(500);
                }
            }

            EnterFromDirection = (ScenePositions)EditorGUILayout.EnumPopup("Enter From", EnterFromDirection);
            FinalPosition = (ScenePositions)EditorGUILayout.EnumPopup("Final Position", FinalPosition);

            EnterTime = EditorGUILayout.Slider("Enter Time", EnterTime, 1f, 10f);
#endif
        }

        public override IEnumerator Execute(UVNFManager managerCallback, UVNFCanvas canvas)
        {
            managerCallback.CharacterManager.AddCharacter(CharacterName, Character, Flip, EnterFromDirection, FinalPosition, EnterTime);
            return null;
        }
    }
}