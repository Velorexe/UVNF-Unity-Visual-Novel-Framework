using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace UVNF.Entities
{
    public class Character : MonoBehaviour
    {
        public string Name;

        public RectTransform Transform;
        public RectTransform Parent;

        public ScenePositions CurrentPosition;
        public Image SpriteRenderer;

        public bool CurrentlyMoving
        {
            get { return movingCoroutine != null; }
        }
        private Coroutine movingCoroutine;

        public void MoveCharacter(Vector2 endPosition, float moveTime)
        {
            if (CurrentlyMoving)
            {
                StopCoroutine(movingCoroutine);
            }

            movingCoroutine = StartCoroutine(MoveCharacterCoroutine(Transform.anchoredPosition, endPosition, moveTime));
        }

        public IEnumerator MoveCharacterCoroutine(Vector2 startPosition, Vector2 endPosition, float moveTime)
        {
            float distance = Vector3.Distance(startPosition, endPosition);
            float currentLerpTime = 0f;

            while (Transform.anchoredPosition != endPosition)
            {
                currentLerpTime += Time.deltaTime;
                if (currentLerpTime > moveTime)
                    currentLerpTime = moveTime;

                float t = currentLerpTime / moveTime;
                t = t * t * t * (t * (6f * t - 15f) + 10f);

                Transform.anchoredPosition = Vector2.Lerp(startPosition, endPosition, t);

                yield return null;
            }
        }

        public void ChangeSprite(Sprite newSprite)
        {
            SpriteRenderer.sprite = newSprite;
            float multiplier = newSprite.rect.height / Transform.rect.height;
            Transform.sizeDelta = new Vector2(newSprite.rect.width / multiplier, Transform.sizeDelta.y);
        }
    }
}