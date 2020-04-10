using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;

public class UDSFCanvas : MonoBehaviour
{
    [Header("Canvas Group")]
    public CanvasGroup BottomPanelCanvasGroup;
    public CanvasGroup ChoiceCanvasGroup;
    public CanvasGroup OveralPanelCanvasGroup;
    public CanvasGroup LoadingCanvasGroup;
    
    [Header("Dialogue")]
    public TextMeshProUGUI DialogueTMP;

    public float TextDisplayInterval = 0.05f;
    private float tempDisplayInterval = 0f;

    private float displayIntervalTimer = 0f;

    [Header("Choices")]
    public GameObject ChoiceButton;
    public Transform ChoicePanelTransform;

    public int ChoiceCallback
    {
        get
        {
            int tempReturn = _choiceCallback;
            _choiceCallback = -1;
            return tempReturn;
        }
        set { _choiceCallback = value; }
    }
    private int _choiceCallback = -1;

    private void Awake()
    {
        //Test
        ShowLoadScreen(1f, true);
    }

    public IEnumerator DisplayText(string text, params TextDisplayStyle[] displayStyles)
    {
        tempDisplayInterval = TextDisplayInterval;
        ResetDialogueTMP();

        for (int i = 0; i < displayStyles.Length; i++)
        {
            switch (displayStyles[i])
            {
                case TextDisplayStyle.Gigantic:
                    DialogueTMP.fontSize = 40f;
                    break;
                case TextDisplayStyle.Big:
                    DialogueTMP.fontSize = 25f;
                    break;
                case TextDisplayStyle.Small:
                    DialogueTMP.fontSize = 12f;
                    break;
                case TextDisplayStyle.Italic:
                    DialogueTMP.fontStyle = FontStyles.Italic;
                    break;
                case TextDisplayStyle.Bold:
                    DialogueTMP.fontStyle = FontStyles.Bold;
                    break;
                case TextDisplayStyle.Fast:
                    tempDisplayInterval = 0.015f;
                    break;
                case TextDisplayStyle.Slow:
                    tempDisplayInterval = 0.075f;
                    break;
            }
        }

        int textIndex = 0;
        while(textIndex < text.Length)
        {
            if (Input.GetMouseButtonUp(0))
            {
                DialogueTMP.text = text;
                textIndex = text.Length;
            }
            else if (displayIntervalTimer >= tempDisplayInterval)
            {
                DialogueTMP.text += text[textIndex];
                textIndex++;
                displayIntervalTimer = 0f;
            }
            else
                displayIntervalTimer += Time.deltaTime;
            yield return null;
        }

        while (!Input.GetMouseButtonUp(0)) yield return null;
        Debug.Log("Finished displaying dialogue.");
    }

    public IEnumerator DisplayChoice(string[] options, params TextDisplayStyle[] displayStyles)
    {
        foreach(Transform child in ChoicePanelTransform)
            Destroy(child.gameObject);

        for (int i = 0; i < options.Length; i++)
        {
            ChoiceButton button = Instantiate(ChoiceButton, ChoicePanelTransform).GetComponent<ChoiceButton>();
            button.Display(options[i], i, this);
        }

        while (_choiceCallback == -1) yield return null;

        foreach (Transform child in ChoicePanelTransform)
            Destroy(child.gameObject);
    }

    public IEnumerator FadeCanvasGroup(CanvasGroup canvasGroup, float time = 1f)
    {
        if (time <= 0f)
        {
            time = 1f;
            Debug.LogWarning("Tried to fade canvas group with a value for time that's less or equal to zero.");
        }

        while (canvasGroup.alpha != 0f)
        {
            canvasGroup.alpha -= Time.deltaTime / time;
            yield return null;
        }
    }

    public IEnumerator UnfadeCanvasGroup(CanvasGroup canvasGroup, float time = 1f)
    {
        if(time <= 0f)
        {
            time = 1f;
            Debug.LogWarning("Tried to unfade canvas group with a value for time that's less or equal to zero.");
        }

        while(canvasGroup.alpha != 1f)
        {
            canvasGroup.alpha += Time.deltaTime / time;
            yield return null;
        }
    }

    public void ShowLoadScreen(float time = 1f, bool hideOtherComponents = false)
    {
        if(time <= 0f)
        {
            time = 1f;
            Debug.LogWarning("Tried to show load screen with a value for time that's less or equal to zero.");
        }

        if (hideOtherComponents)
            StartCoroutine(FadeCanvasGroup(OveralPanelCanvasGroup, time));
        StartCoroutine(UnfadeCanvasGroup(LoadingCanvasGroup, time));
    }

    private void ResetDialogueTMP()
    {
        DialogueTMP.fontSize = 18f;
        DialogueTMP.fontStyle = 0;
        DialogueTMP.text = string.Empty;

        tempDisplayInterval = TextDisplayInterval;
    }
}
