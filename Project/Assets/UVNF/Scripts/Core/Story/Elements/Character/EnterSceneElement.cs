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

    public override void DisplayLayout()
    {
#if UNITY_EDITOR
        GUILayout.BeginHorizontal();
        GUILayout.Label("Character Sprite");
        Character = EditorGUILayout.ObjectField(Character, typeof(Sprite), true) as Sprite;
        GUILayout.EndHorizontal();

        Flip = GUILayout.Toggle(Flip, "Flip");

        Rect lastRect = GUILayoutUtility.GetLastRect();
        if (Character != null)
        {
            foldOut = EditorGUILayout.Foldout(foldOut, "Preview", true);
            if (foldOut)
            {
                lastRect.position = new Vector2(lastRect.position.x, lastRect.position.y + 10);
                lastRect.width = 1000;
                lastRect.height = 500;

                GUILayout.BeginArea(lastRect);
                {
                    lastRect.width = Character.rect.width / (Character.rect.height / 500);
                    //if (Flip) lastRect.width = -lastRect.width;
                    lastRect.height = 500;
                    GUI.DrawTexture(lastRect, Character.texture, ScaleMode.ScaleToFit);
                }
                GUILayout.EndArea();
                GUILayout.Space(400);
            }
        }

        EnterFromDirection = (ScenePositions)EditorGUILayout.EnumPopup("Enter From", EnterFromDirection);
        FinalPosition = (ScenePositions)EditorGUILayout.EnumPopup("Final Position", FinalPosition);
#endif
    }

    public override IEnumerator Execute(GameManager managerCallback, UVNFCanvas canvas)
    {
        yield return null;
    }
}
