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

    public void AddCharacter(Sprite characterSprite, bool flip, ScenePositions enter, ScenePositions position, float enterTime)
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
        
        Vector3 startPosition = new Vector3();
        switch (enter)
        {
            case ScenePositions.Left:
                startPosition = new Vector3(characterSprite.rect.width - parentTransform.position.x + parentTransform.rect.x, 0, 0);
                break;
            case ScenePositions.Top:
                startPosition = new Vector3(0, parentTransform.position.y + parentTransform.rect.y + characterSprite.rect.height, 0);
                break;
            case ScenePositions.Right:
                startPosition = new Vector3(parentTransform.anchoredPosition.x + characterSprite.rect.width, 0, 0);
                break;
        }

        Image spriteShower = obj.AddComponent<Image>();
        spriteShower.sprite = characterSprite;
        spriteShower.preserveAspect = true;

        RectTransform spriteTransform = obj.GetComponent<RectTransform>();
        if (flip)
            spriteTransform.localScale = spriteTransform.localScale.SetX(-spriteTransform.localScale.x);
        spriteTransform.sizeDelta = LeftSideCharacterStack.GetComponent<RectTransform>().sizeDelta;

        CharactersOnScreen.Add(spriteTransform);

        //TODO: Calculate with already set characters
        Vector3 endPosition = new Vector3(0, 0, 0);
        StartCoroutine(MoveCharacter(spriteTransform, startPosition, endPosition, enterTime));
    }

    private IEnumerator MoveCharacter(RectTransform character, Vector2 startPosition, Vector2 endPosition, float enterTime)
    {
        float time = enterTime;

        float distance = Vector3.Distance(startPosition, endPosition);
        float currentLerpTime = 0f;

        while(character.anchoredPosition != endPosition)
        {
            currentLerpTime += Time.deltaTime;
            if (currentLerpTime > time)
                currentLerpTime = time;

            float t = currentLerpTime / time;
            t = t * t * t * (t * (6f * t - 15f) + 10f);
            character.anchoredPosition = Vector2.Lerp(startPosition, endPosition, t);
            yield return null;
        }
    }
}
