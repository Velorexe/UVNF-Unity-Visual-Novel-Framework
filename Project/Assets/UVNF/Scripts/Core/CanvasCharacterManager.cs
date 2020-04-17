using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasCharacterManager : MonoBehaviour
{
    public List<Transform> CharactersOnScreen;

    public Transform LeftSideCharacterStack;
    private List<Transform> _leftSideCharacters;

    public Transform MiddleCharacterStack;
    private List<Transform> _middleSideCharacters;

    public Transform RightSideCharacterStack;
    private List<Transform> _rightSideCharacters;

    public void AddCharacter(Sprite characterSprite, bool flip, ScenePositions enter, ScenePositions position)
    {
        GameObject obj = new GameObject(characterSprite.name, typeof(RectTransform));
        RectTransform parentTransform = null;
        switch (position)
        {
            case ScenePositions.Left:
                obj.transform.SetParent(LeftSideCharacterStack);
                parentTransform = LeftSideCharacterStack.GetComponent<RectTransform>();
                break;
            case ScenePositions.Middle:
                obj.transform.SetParent(MiddleCharacterStack);
                parentTransform = MiddleCharacterStack.GetComponent<RectTransform>();
                break;
            case ScenePositions.Right:
                obj.transform.SetParent(RightSideCharacterStack);
                parentTransform = RightSideCharacterStack.GetComponent<RectTransform>();
                break;
            default: break;
        }

        Image spriteShower = obj.AddComponent<Image>();
        spriteShower.sprite = characterSprite;
        spriteShower.SetNativeSize();


        RectTransform spriteTransform = obj.GetComponent<RectTransform>();
        if (flip)
            spriteTransform.localScale = spriteTransform.localScale.SetX(-spriteTransform.localScale.x);

        CharactersOnScreen.Add(spriteTransform);
        
        Vector3 startPosition = new Vector3(characterSprite.rect.width - parentTransform.position.x + parentTransform.rect.x, 0, 0);
        //TODO: Calculate with already set characters
        Vector3 endPosition = new Vector3(0, 0, 0);
        StartCoroutine(MoveCharacter(spriteTransform, startPosition, endPosition));
    }

    private IEnumerator MoveCharacter(RectTransform character, Vector2 startPosition, Vector2 endPosition)
    {
        float speed = 100f;

        float distance = Vector3.Distance(startPosition, endPosition);
        float startTime = Time.time;

        while(character.anchoredPosition != endPosition)
        {
            float distCovered = (Time.time - startTime) * speed;
            float fractionOfJourney = distCovered / distance;

            character.anchoredPosition = Vector2.Lerp(startPosition, endPosition, fractionOfJourney);
            yield return null;
        }
    }
}
