using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasCharacterManager : MonoBehaviour
{
    public List<Character> CharactersOnScreen;

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
                startPosition = new Vector3(-spriteTransform.rect.width * 1.5f/*- parentTransform.position.x*/, 0, 0);
                break;
            case ScenePositions.Top:
                startPosition = new Vector3(0, spriteTransform.rect.height, 0);
                break;
            case ScenePositions.Right:
                startPosition = new Vector3(-parentTransform.anchoredPosition.x + spriteTransform.rect.width, 0, 0);
                break;
        }

        spriteTransform.anchoredPosition = startPosition;

        Character character = obj.AddComponent<Character>();
        character.Name = characterName;
        character.Transform = spriteTransform;
        character.Parent = parentTransform;

        CharactersOnScreen.Add(character);
        _leftSideCharacters.Add(characterName, spriteTransform);

        //TODO: Calculate with already set characters
        Vector3 endPosition = new Vector3(0, 0, 0);
        character.MoveCharacter(endPosition, enterTime);
    }

    public void RemoveCharacter(string characterName, ScenePositions exitPosition, float exitTime)
    {
        Character character = CharactersOnScreen.Find(x => x.Name == characterName);

        Vector3 endPosition = new Vector3();

        switch (exitPosition)
        {
            case ScenePositions.Left:
                endPosition = new Vector3(-character.Transform.rect.width * 1.5f, 0, 0);
                break;
            case ScenePositions.Top:
                endPosition = new Vector3(0, character.Parent.position.y + character.Parent.rect.y + character.Transform.rect.height, 0);
                break;
            case ScenePositions.Right:
                endPosition = new Vector3(character.Parent.anchoredPosition.x + character.Transform.rect.width, 0, 0);
                break;
        }

        switch (character.CurrentPosition)
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
        character.MoveCharacter(endPosition, exitTime);
    }
}
