using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public string Name;
    public RectTransform Transform;
    public RectTransform Parent;

    public ScenePositions CurrentPosition;

    public bool CurrentlyMoving
    {
        get { return movingCoroutine != null; }
    }
    private Coroutine movingCoroutine;

    public void MoveCharacter(Vector2 endPosition, float moveTime)
    {
        if (CurrentlyMoving)
            StopCoroutine(movingCoroutine);

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
}
