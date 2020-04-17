using System.Collections;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UVNFCanvas : MonoBehaviour
{
    [Header("Canvas Group")]
    public CanvasGroup BottomCanvasGroup;
    public CanvasGroup ChoiceCanvasGroup;
    public CanvasGroup LoadingCanvasGroup;
    public CanvasGroup BackgroundCanvasGroup;
    
    [Header("Dialogue")]
    public TextMeshProUGUI DialogueTMP;
    public TextMeshProUGUI CharacterTMP;

    public float TextDisplayInterval = 0.05f;
    private float tempDisplayInterval = 0f;

    private float displayIntervalTimer = 0f;

    [Header("Choices")]
    public GameObject ChoiceButton;
    public Transform ChoicePanelTransform;

    [Header("Background")]
    public Image BackgroundImage;
    public Image BackgroundFade;

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

    public bool BottomPanelEnabled => BottomCanvasGroup.gameObject.activeSelf;
    public bool ChoiceCanvasEnabled => ChoiceCanvasGroup.gameObject.activeSelf;
    public bool LoadingCanvasEnabled => LoadingCanvasGroup.gameObject.activeSelf;

    private void Awake()
    {
        if(BackgroundCanvasGroup != null)
            BackgroundCanvasGroup.gameObject.SetActive(true);
        if(ChoiceCanvasGroup != null)
            ChoiceCanvasGroup.gameObject.SetActive(false);
        if(BottomCanvasGroup != null)
            BottomCanvasGroup.gameObject.SetActive(false);
    }

    #region Dialogue
    public IEnumerator DisplayText(string text, params TextDisplayStyle[] displayStyles)
    {
        ApplyTextDisplayStylesToTMP(DialogueTMP, displayStyles);
        BottomCanvasGroup.gameObject.SetActive(true);

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
    }

    public IEnumerator DisplayText(string text, string characterName, bool useStylesForCharacterField = false, params TextDisplayStyle[] displayStyles)
    {
        ApplyTextDisplayStylesToTMP(DialogueTMP, displayStyles);
        if (useStylesForCharacterField)
            ApplyTextDisplayStylesToTMP(CharacterTMP, displayStyles);

        BottomCanvasGroup.gameObject.SetActive(true);

        if (!string.Equals(CharacterTMP.text, characterName, StringComparison.Ordinal))
            CharacterTMP.text = characterName;

        int textIndex = 0;
        while (textIndex < text.Length)
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
    }
    
    public IEnumerator DisplayText(string text, string characterName, AudioClip dialogue, AudioManager audio, bool useStylesForCharacterField = false, params TextDisplayStyle[] displayStyles)
    {
        ApplyTextDisplayStylesToTMP(DialogueTMP, displayStyles);
        if (useStylesForCharacterField)
            ApplyTextDisplayStylesToTMP(CharacterTMP, displayStyles);

        BottomCanvasGroup.gameObject.SetActive(true);

        if (!string.Equals(CharacterTMP.text, characterName, StringComparison.Ordinal))
            CharacterTMP.text = characterName;

        audio.PlaySound(dialogue, 1f);

        int textIndex = 0;
        while (textIndex < text.Length)
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
    }

    public IEnumerator DisplayText(string text, string characterName, AudioClip boop, AudioManager audio, float pitchShift, bool beepOnVowel = false, bool useStylesForCharacterField = false, params TextDisplayStyle[] displayStyles)
    {
        ApplyTextDisplayStylesToTMP(DialogueTMP, displayStyles);
        if (useStylesForCharacterField)
            ApplyTextDisplayStylesToTMP(CharacterTMP, displayStyles);

        BottomCanvasGroup.gameObject.SetActive(true);

        if (!string.Equals(CharacterTMP.text, characterName, StringComparison.Ordinal))
            CharacterTMP.text = characterName;

        int textIndex = 0;
        while (textIndex < text.Length)
        {
            if (Input.GetMouseButtonUp(0))
            {
                DialogueTMP.text = text;
                textIndex = text.Length;
            }
            else if (displayIntervalTimer >= tempDisplayInterval)
            {
                DialogueTMP.text += text[textIndex];

                if (text[textIndex] != ' ')
                {
                    if(beepOnVowel && text[textIndex].IsVowel())
                        audio.PlaySound(boop, 1f, UnityEngine.Random.Range(0, pitchShift));
                    else if(!beepOnVowel)
                        audio.PlaySound(boop, 1f, UnityEngine.Random.Range(0, pitchShift));
                }

                textIndex++;
                displayIntervalTimer = 0f;

            }
            else
                displayIntervalTimer += Time.deltaTime;
            yield return null;
        }

        while (!Input.GetMouseButtonUp(0)) yield return null;
    }

    #endregion

    #region Choice
    public void DisplayChoice(string[] options, bool hideDialogue = true , params TextDisplayStyle[] displayStyles)
    {
        StartCoroutine(DisplayChoiceCoroutine(options, hideDialogue, displayStyles));
    }

    public IEnumerator DisplayChoiceCoroutine(string[] options, bool hideDialogue = true, params TextDisplayStyle[] displayStyles)
    {
        BottomCanvasGroup.gameObject.SetActive(!hideDialogue);
        ChoiceCanvasGroup.gameObject.SetActive(true);

        foreach(Transform child in ChoicePanelTransform)
            Destroy(child.gameObject);

        for (int i = 0; i < options.Length; i++)
        {
            ChoiceButton button = Instantiate(ChoiceButton, ChoicePanelTransform).GetComponent<ChoiceButton>();
            button.Display(options[i], i, this);
        }

        while (_choiceCallback == -1) yield return null;

        ChoiceCanvasGroup.gameObject.SetActive(false);

        foreach (Transform child in ChoicePanelTransform)
            Destroy(child.gameObject);
    }
    #endregion

    #region Utility
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
        canvasGroup.gameObject.SetActive(false);
    }

    public IEnumerator UnfadeCanvasGroup(CanvasGroup canvasGroup, float time = 1f)
    {
        canvasGroup.gameObject.SetActive(true);

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
        {
            StartCoroutine(FadeCanvasGroup(BottomCanvasGroup, time));
            StartCoroutine(FadeCanvasGroup(ChoiceCanvasGroup, time));
        }
        StartCoroutine(UnfadeCanvasGroup(LoadingCanvasGroup, time));
    }

    public void HideLoadScreen(float time = 1f, bool showOtherComponents = false)
    {
        if (time <= 0f)
        {
            time = 1f;
            Debug.LogWarning("Tried to show load screen with a value for time that's less or equal to zero.");
        }

        if (showOtherComponents)
        {
            StartCoroutine(UnfadeCanvasGroup(BottomCanvasGroup, time));
            StartCoroutine(UnfadeCanvasGroup(ChoiceCanvasGroup, time));
        }
        StartCoroutine(FadeCanvasGroup(LoadingCanvasGroup, time));
    }
    #endregion

    #region Scenery
    public void ChangeBackground(Sprite newBackground)
    {
        BackgroundCanvasGroup.gameObject.SetActive(true);
        BackgroundImage.sprite = newBackground;
    }

    public void ChangeBackground(Sprite newBackground, float transitionTime)
    {
        BackgroundCanvasGroup.gameObject.SetActive(true);
        Color32 alpha = BackgroundFade.color;
        alpha.a = 255;
        BackgroundFade.color = alpha;

        BackgroundFade.sprite = BackgroundImage.sprite;
        BackgroundImage.sprite = newBackground;

        BackgroundFade.canvasRenderer.SetAlpha(1f);
        BackgroundFade.CrossFadeAlpha(0f, transitionTime, false);
    }
    #endregion

    private void ApplyTextDisplayStylesToTMP(TextMeshProUGUI tmp, TextDisplayStyle[] displayStyles)
    {
        ResetTMP(tmp);
        for (int i = 0; i < displayStyles.Length; i++)
        {
            switch (displayStyles[i])
            {
                case TextDisplayStyle.Gigantic:
                    tmp.fontSize = 40f;
                    break;
                case TextDisplayStyle.Big:
                    tmp.fontSize = 25f;
                    break;
                case TextDisplayStyle.Small:
                    tmp.fontSize = 12f;
                    break;
                case TextDisplayStyle.Italic:
                    tmp.fontStyle = FontStyles.Italic;
                    break;
                case TextDisplayStyle.Bold:
                    tmp.fontStyle = FontStyles.Bold;
                    break;
                case TextDisplayStyle.Fast:
                    tempDisplayInterval = TextDisplayInterval * 3f;
                    break;
                case TextDisplayStyle.Slow:
                    tempDisplayInterval = TextDisplayInterval / 2f;
                    break;
            }
        }
    }

    private void ResetTMP(TextMeshProUGUI tmp)
    {
        tmp.fontSize = 18f;
        tmp.fontStyle = 0;
        tmp.text = string.Empty;

        tempDisplayInterval = TextDisplayInterval;
    }
}
