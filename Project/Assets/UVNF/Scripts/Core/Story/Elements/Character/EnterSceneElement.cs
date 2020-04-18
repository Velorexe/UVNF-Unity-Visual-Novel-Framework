using System.Collections;
using UnityEditor;
using UnityEngine;

public class EnterSceneElement : StoryElement
{
    public override string ElementName => "Enter Scene";

    public override Color32 DisplayColor => _displayColor;
    private Color32 _displayColor = new Color32().Character();

    public override StoryElementTypes Type => StoryElementTypes.Character;

    public Sprite Character;
    private bool foldOut = false;

    public bool Flip = false;

    public ScenePositions EnterFromDirection = ScenePositions.Left;
    public ScenePositions FinalPosition = ScenePositions.Left;

    public float EnterTime = 2f;

    public override void DisplayLayout(Rect layoutRect)
    {
#if UNITY_EDITOR
        GUILayout.BeginHorizontal();
        GUILayout.Label("Character Sprite");
        Character = EditorGUILayout.ObjectField(Character, typeof(Sprite), true) as Sprite;
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

        EnterTime = EditorGUILayout.Slider(EnterTime, 1f, 10f);
#endif
    }

    public override IEnumerator Execute(GameManager managerCallback, UVNFCanvas canvas)
    {
        managerCallback.CharacterManager.AddCharacter(Character, Flip, EnterFromDirection, FinalPosition, EnterTime);
        return null;
    }
}
