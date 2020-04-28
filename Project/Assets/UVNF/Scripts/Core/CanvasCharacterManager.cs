using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasCharacterManager : MonoBehaviour
{
    public List<Character> CharactersOnScreen;
    public RectTransform MainCharacterStack;

    private Character[] LeftSideCharacters { get { return CharactersOnScreen.Where(x => x.CurrentPosition == ScenePositions.Left).ToArray(); } }
    private Character[] MiddleSideCharacters { get { return CharactersOnScreen.Where(x => x.CurrentPosition == ScenePositions.Middle).ToArray(); } }
    private Character[] RightSideCharacters { get { return CharactersOnScreen.Where(x => x.CurrentPosition == ScenePositions.Right).ToArray(); } }

    public void AddCharacter(string characterName, Sprite characterSprite, bool flip, ScenePositions enter, ScenePositions position, float enterTime)
    {
        GameObject obj = new GameObject(characterSprite.name, typeof(RectTransform));
        RectTransform parentTransform = null;

        Image spriteShower = obj.AddComponent<Image>();
        spriteShower.sprite = characterSprite;
        spriteShower.preserveAspect = true;

        RectTransform spriteTransform = obj.GetComponent<RectTransform>();
        obj.transform.SetParent(MainCharacterStack);
        parentTransform = MainCharacterStack.GetComponent<RectTransform>();
        spriteTransform.sizeDelta = MainCharacterStack.GetComponent<RectTransform>().sizeDelta;

        if (flip)
            spriteTransform.localScale = new Vector3(-1, 1, 1);
        else
            spriteTransform.localScale = new Vector3(1, 1, 1);

        Character character = obj.AddComponent<Character>();
        character.Name = characterName;
        character.Transform = spriteTransform;
        character.Parent = parentTransform;
        character.CurrentPosition = position;

        float multiplier = characterSprite.rect.height / spriteTransform.rect.height;
        spriteTransform.sizeDelta = new Vector2(characterSprite.rect.width / multiplier, spriteTransform.sizeDelta.y);

        Vector2 startPosition = new Vector2();
        switch (enter)
        {
            case ScenePositions.Left:
                startPosition = new Vector2(-parentTransform.sizeDelta.x - spriteTransform.sizeDelta.x / 2, 0);
                break;
            case ScenePositions.Top:
                startPosition = new Vector2(0, parentTransform.sizeDelta.y + spriteTransform.sizeDelta.y / 2);
                break;
            case ScenePositions.Right:
                startPosition = new Vector2(parentTransform.sizeDelta.x + spriteTransform.sizeDelta.x / 2, 0);
                break;
        }

        spriteTransform.anchoredPosition = startPosition;
        
        Vector2 endPosition = new Vector2();     
        switch (position)
        {
            case ScenePositions.Left:
                endPosition = new Vector2(-(parentTransform.sizeDelta.x / 2), 0);
                CharactersOnScreen.Add(character);

                Character[] leftCharacters = LeftSideCharacters.Reverse().ToArray();
                if (leftCharacters.Length > 1)
                {
                    float leftPosition = Mathf.Abs(parentTransform.sizeDelta.x);
                    float offset = leftPosition / (leftCharacters.Length + 1);
                    for (int i = 0; i < leftCharacters.Length; i++)
                    {
                        Vector2 newPosition = new Vector2(-parentTransform.sizeDelta.x + offset * (i + 1), 0);
                        leftCharacters[i].MoveCharacter(newPosition, 1f);
                    }
                }
                else
                {
                    character.MoveCharacter(endPosition, enterTime);
                }
                break;
            case ScenePositions.Top:
                endPosition = new Vector2(0, 0);
                break;
            case ScenePositions.Right:
                endPosition = new Vector2(parentTransform.sizeDelta.x / 2, 0);
                CharactersOnScreen.Add(character);

                Character[] rightCharacters = RightSideCharacters;
                if (rightCharacters.Length > 1)
                {
                    float rightPosition = Mathf.Abs(parentTransform.sizeDelta.x);
                    float offset = rightPosition / (rightCharacters.Length + 1);
                    for (int i = 0; i < rightCharacters.Length; i++)
                    {
                        Vector2 newPosition = new Vector2(offset * (i + 1), 0);
                        rightCharacters[i].MoveCharacter(newPosition, 1f);
                    }
                }
                else
                {
                    character.MoveCharacter(endPosition, enterTime);
                }
                break;
        }

    }

    public void RemoveCharacter(string characterName, ScenePositions exitPosition, float exitTime)
    {
        Character character = CharactersOnScreen.Find(x => x.Name == characterName);

        Vector3 endPosition = new Vector3();

        switch (exitPosition)
        {
            case ScenePositions.Left:
                endPosition = new Vector3(-(character.Parent.rect.width + (character.Transform.rect.width / 2f)), 0, 0);
                break;
            case ScenePositions.Top:
                endPosition = new Vector3(0, character.Transform.rect.height, 0);
                break;
            case ScenePositions.Right:
                endPosition = new Vector3(character.Parent.rect.width + (character.Transform.rect.width / 2f), 0, 0);
                break;
        }

        CharactersOnScreen.Remove(character);
        character.MoveCharacter(endPosition, exitTime);
    }

    public void MoveCharacterTo(string characterName, string characterToMoveTo, float moveTime)
    {
        Character mainCharacter = CharactersOnScreen.Find(x => x.Name == characterName);
        Character moveToCharacter = CharactersOnScreen.Find(x => x.Name == characterToMoveTo);

        mainCharacter.MoveCharacter(moveToCharacter.Transform.anchoredPosition, moveTime);
    }
}
