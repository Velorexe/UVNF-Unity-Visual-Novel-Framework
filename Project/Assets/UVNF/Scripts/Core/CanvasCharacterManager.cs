using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasCharacterManager : MonoBehaviour
{
    public List<Transform> CharactersOnScreen;

    public RectTransform LeftSideCharacterStack;
    private Dictionary<string, Transform> _leftSideCharacters = new Dictionary<string, Transform>();

    public RectTransform MiddleCharacterStack;
    private Dictionary<string, Transform> _middleSideCharacters = new Dictionary<string, Transform>();

    public RectTransform RightSideCharacterStack;
    private Dictionary<string, Transform> _rightSideCharacters = new Dictionary<string, Transform>();

    public void AddCharacter(string characterName, Sprite characterSprite, bool flip, ScenePositions enter, ScenePositions position, float enterTime)
    {
        GameObject obj = new GameObject(characterSprite.name, typeof(RectTransform));
        RectTransform parentTransform = null;

        Image spriteShower = obj.AddComponent<Image>();
        spriteShower.sprite = characterSprite;
        spriteShower.preserveAspect = true;

        RectTransform spriteTransform = obj.GetComponent<RectTransform>();

        switch (position)
        {
            case ScenePositions.Left:
                obj.transform.SetParent(LeftSideCharacterStack);
                parentTransform = LeftSideCharacterStack.GetComponent<RectTransform>();
                spriteTransform.sizeDelta = LeftSideCharacterStack.GetComponent<RectTransform>().sizeDelta;
                break;
            case ScenePositions.Middle:
                obj.transform.SetParent(MiddleCharacterStack);
                parentTransform = MiddleCharacterStack.GetComponent<RectTransform>();
                spriteTransform.sizeDelta = MiddleCharacterStack.GetComponent<RectTransform>().sizeDelta;
                break;
            case ScenePositions.Right:
                obj.transform.SetParent(RightSideCharacterStack);
                parentTransform = RightSideCharacterStack.GetComponent<RectTransform>();
                spriteTransform.sizeDelta = RightSideCharacterStack.GetComponent<RectTransform>().sizeDelta;
                break;
            default: break;
        }

        if (flip)
            spriteTransform.localScale = new Vector3(-1, 1, 1);
        else
            spriteTransform.localScale = new Vector3(1, 1, 1);

        Vector3 startPosition = new Vector3();
        switch (enter)
        {
            case ScenePositions.Left:
                startPosition = new Vector3(characterSprite.rect.width - parentTransform.position.x, 0, 0);
                break;
            case ScenePositions.Top:
                startPosition = new Vector3(0, parentTransform.position.y + parentTransform.rect.y + characterSprite.rect.height, 0);
                break;
            case ScenePositions.Right:
                startPosition = new Vector3(parentTransform.anchoredPosition.x + characterSprite.rect.width, 0, 0);
                break;
        }

        CharactersOnScreen.Add(spriteTransform);
        _leftSideCharacters.Add(characterName, spriteTransform);

        //TODO: Calculate with already set characters
        Vector3 endPosition = new Vector3(0, 0, 0);
        StartCoroutine(MoveCharacter(spriteTransform, startPosition, endPosition, enterTime));
    }

    public void RemoveCharacter(string characterName, ScenePositions exitPosition, float exitTime)
    {
        Tuple<RectTransform, RectTransform> characterResult = GetCharacter(characterName);
        
        RectTransform characterSprite = characterResult.Item1;
        RectTransform parentTransform = characterResult.Item2;

        Vector3 endPosition = new Vector3();

        switch (exitPosition)
        {
            case ScenePositions.Left:
                endPosition = new Vector3(-characterSprite.rect.width /*- parentTransform.position.x*/, 0, 0);
                break;
            case ScenePositions.Top:
                endPosition = new Vector3(0, parentTransform.position.y + parentTransform.rect.y + characterSprite.rect.height, 0);
                break;
            case ScenePositions.Right:
                endPosition = new Vector3(parentTransform.anchoredPosition.x + characterSprite.rect.width, 0, 0);
                break;
        }

        ScenePositions characterPosition = GetCharacterPosition(characterName);
        switch (characterPosition)
        {
            case ScenePositions.Left:
                _leftSideCharacters.Remove(characterName);
                break;
            case ScenePositions.Middle:
                _middleSideCharacters.Remove(characterName);
                break;
            case ScenePositions.Right:
                _rightSideCharacters.Remove(characterName);
                break;
        }
        StartCoroutine(MoveCharacter(characterSprite, characterSprite.anchoredPosition, endPosition, exitTime));
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

    private Tuple<RectTransform, RectTransform> GetCharacter(string characterName)
    {
        if (_leftSideCharacters.ContainsKey(characterName))
            return new Tuple<RectTransform, RectTransform>(_leftSideCharacters[characterName].GetComponent<RectTransform>(), LeftSideCharacterStack);
        if (_middleSideCharacters.ContainsKey(characterName))
            return new Tuple<RectTransform, RectTransform>(_middleSideCharacters[characterName].GetComponent<RectTransform>(), MiddleCharacterStack);
        if (_rightSideCharacters.ContainsKey(characterName))
            return new Tuple<RectTransform, RectTransform>(_rightSideCharacters[characterName].GetComponent<RectTransform>(), RightSideCharacterStack);
        return null;
    }

    private ScenePositions GetCharacterPosition(string characterName)
    {
        if (_leftSideCharacters.ContainsKey(characterName))
            return ScenePositions.Left;
        if (_middleSideCharacters.ContainsKey(characterName))
            return ScenePositions.Middle;
        if (_rightSideCharacters.ContainsKey(characterName))
            return ScenePositions.Right;
        return ScenePositions.Middle;
    }
}
