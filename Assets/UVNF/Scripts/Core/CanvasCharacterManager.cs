using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UVNF.Entities;

namespace UVNF.Core
{
    /// <summary>
    /// Manages characters being shown on the screen, including their position on a name / key basis
    /// </summary>
    public class CanvasCharacterManager : MonoBehaviour
    {
        // TODO: replace all of this with a float system (a character should be placed on { x: 0.1f, y: 0.5f }

        public List<Character> CharactersOnScreen;
        public RectTransform MainCharacterStack;

        private Character[] LeftSideCharacters { get { return CharactersOnScreen.Where(x => x.CurrentPosition == ScenePositions.Left).ToArray(); } }
        private Character[] MiddleSideCharacters { get { return CharactersOnScreen.Where(x => x.CurrentPosition == ScenePositions.Middle).ToArray(); } }
        private Character[] RightSideCharacters { get { return CharactersOnScreen.Where(x => x.CurrentPosition == ScenePositions.Right).ToArray(); } }

        public void AddCharacter(string characterName, Sprite characterSprite, bool flip, ScenePositions enter, ScenePositions position, float enterTime)
        {
            MainCharacterStack.gameObject.SetActive(true);

            GameObject obj = new GameObject(characterSprite.name, typeof(RectTransform));
            RectTransform parentTransform;

            Image spriteRenderer = obj.AddComponent<Image>();
            spriteRenderer.sprite = characterSprite;
            spriteRenderer.preserveAspect = true;

            RectTransform spriteTransform = obj.GetComponent<RectTransform>();
            obj.transform.SetParent(MainCharacterStack);
            parentTransform = MainCharacterStack;
            spriteTransform.sizeDelta = MainCharacterStack.sizeDelta;

            if (flip)
            {
                spriteTransform.localScale = new Vector3(-1, 1, 1);
            }
            else
            {
                spriteTransform.localScale = new Vector3(1, 1, 1);
            }

            Character character = obj.AddComponent<Character>();

            character.Name = characterName;
            character.Transform = spriteTransform;
            character.Parent = parentTransform;
            character.CurrentPosition = position;
            character.SpriteRenderer = spriteRenderer;

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
            CharactersOnScreen.Add(character);

            Vector2 endPosition = new Vector2();
            switch (position)
            {
                case ScenePositions.Left:
                    endPosition = new Vector2(-(parentTransform.sizeDelta.x / 2), 0);

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
                    character.MoveCharacter(endPosition, enterTime);
                    break;
                case ScenePositions.Middle:
                    endPosition = new Vector2(0f, 0f);
                    character.MoveCharacter(endPosition, enterTime);
                    break;
                case ScenePositions.Right:
                    endPosition = new Vector2(parentTransform.sizeDelta.x / 2, 0);

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

        public void ChangeCharacterSprite(string characterName, Sprite characterSprite)
        {
            Character character = CharactersOnScreen.Find(x => x.Name == characterName);
            character.ChangeSprite(characterSprite);
        }

        public void Hide()
        {
            MainCharacterStack.gameObject.SetActive(false);
        }
    }
}
