using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.InputSystem.InputAction;

namespace UVNF.Core.UI
{
    /// <summary>
    /// Manages the UI of UVNF. Holds references to the different canvasses, manages
    /// the dialogue, choices and backgrounds that are displayed in the UI of UVNF.
    /// </summary>
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

        public GameObject CharacterNamePlate;

        // The default interval between letters displayed during dialogue
        public readonly float TextDisplayInterval = 0.05f;
        // The "manipulated" interval (i.e. when the regular interval is set higher / lower)
        private float tempDisplayInterval = 0f;

        // Timer keeping track of the time inbetween letters being displayed
        private float displayIntervalTimer = 0f;

        [Header("Choices")]
        public GameObject ChoiceButton;
        public Transform ChoicePanelTransform;

        [Header("Background")]
        public Image BackgroundImage;
        public Image BackgroundFade;

        /// <summary>
        /// The index of the choice that's read from a callback from a choice being displayed
        /// </summary>
        public int ChoiceCallback = -1;

        /// <summary>
        /// Resets the <see cref="ChoiceCallback"/> to -1 (default)
        /// </summary>
        public void ResetChoice() => ChoiceCallback = -1;

        /// <summary>
        /// <see langword="true"/> if there's currently an input.
        /// Resets to <see langword="false"/> when <see langword="get"/> is called
        /// </summary>
        private bool HasInput
        {
            get
            {
                if (_hasInput)
                {
                    _hasInput = false;
                    return true;
                }

                return false;
            }
            set { _hasInput = value; }
        }

        private bool _hasInput;

        private void Awake()
        {
            if (BackgroundCanvasGroup != null)
            {
                BackgroundCanvasGroup.gameObject.SetActive(true);
            }

            if (ChoiceCanvasGroup != null)
            {
                ChoiceCanvasGroup.gameObject.SetActive(false);
            }

            if (BottomCanvasGroup != null)
            {
                BottomCanvasGroup.gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// Processes Unity's Input System's <see cref="CallbackContext"/> if an input occurs
        /// </summary>
        /// <param name="ctx"></param>
        public void ProcessInput(CallbackContext ctx)
        {
            HasInput = ctx.performed;
        }

        #region Dialogue

        /// <summary>
        /// Displays the given dialogue without a character nameplate
        /// </summary>
        /// <param name="text">The dialogue that should be displayed</param>
        /// <param name="displayStyles">A collection of display styles that affect the look of the dialogue</param>
        /// <returns>A Unity <see cref="Coroutine"/></returns>
        public IEnumerator DisplayText(string text, params TextDisplayStyle[] displayStyles)
        {
            ApplyTextDisplayStylesToTMP(DialogueTMP, displayStyles);
            BottomCanvasGroup.gameObject.SetActive(true);

            CharacterNamePlate.SetActive(false);

            int textIndex = 0;
            while (textIndex < text.Length)
            {
                // If there's input, display all of the text at once
                if (HasInput)
                {
                    DialogueTMP.text = text;
                    textIndex = text.Length - 1;
                }
                // Else if the timer is over the time it should take
                // for a character to be shown, show a character
                else if (displayIntervalTimer >= tempDisplayInterval)
                {
                    DialogueTMP.text += ApplyTypography(text, ref textIndex);
                    textIndex++;
                    displayIntervalTimer = 0f;
                }
                else
                {
                    displayIntervalTimer += Time.deltaTime;
                }

                yield return null;
            }

            // Wait for input again before proceeding to the next story element
            while (!HasInput)
            {
                yield return null;
            }
        }

        /// <summary>
        /// Displays the given dialogue with a character nameplate
        /// </summary>
        /// <param name="text">The dialogue that should be displayed</param>
        /// <param name="characterName">The name of the character that should be displayed on the nameplate</param>
        /// <param name="useStylesForCharacterField"><see langword="true"/> if the nameplate should use the given styles in <paramref name="displayStyles"/></param>
        /// <param name="displayStyles">A collection of display styles that affect the look of the dialogue</param>
        /// <returns>A Unity <see cref="Coroutine"/></returns>
        public IEnumerator DisplayText(string text, string characterName, bool useStylesForCharacterField = false, params TextDisplayStyle[] displayStyles)
        {
            if (useStylesForCharacterField)
            {
                ApplyTextDisplayStylesToTMP(CharacterTMP, displayStyles);
            }

            if (!string.Equals(CharacterTMP.text, characterName, StringComparison.Ordinal))
            {
                CharacterTMP.text = characterName;
            }

            CharacterNamePlate.SetActive(!string.IsNullOrEmpty(characterName));

            return DisplayText(text, displayStyles);
        }

        /// <summary>
        /// Displays trhe given dialogue with a character nameplate, accompanied by an <see cref="AudioClip"/>
        /// </summary>
        /// <param name="text">The dialogue that should be displayed</param>
        /// <param name="characterName">The name of the character that should be displayed on the nameplate</param>
        /// <param name="dialogue">The audio that should play during the dialogue</param>
        /// <param name="audio">The <see cref="AudioManager"/> that should play the <paramref name="dialogue"/></param>
        /// <param name="useStylesForCharacterField"><see langword="true"/> if the nameplate should use the given styles</param>
        /// <param name="displayStyles">A collection of display styles that affect the look of the dialogue</param>
        /// <returns>A Unity <see cref="Coroutine"/></returns>
        public IEnumerator DisplayText(string text, string characterName, AudioClip dialogue, AudioManager audio, bool useStylesForCharacterField = false, params TextDisplayStyle[] displayStyles)
        {
            ApplyTextDisplayStylesToTMP(DialogueTMP, displayStyles);
            if (useStylesForCharacterField)
            {
                ApplyTextDisplayStylesToTMP(CharacterTMP, displayStyles);
            }

            CharacterNamePlate.SetActive(!string.IsNullOrEmpty(characterName));

            BottomCanvasGroup.gameObject.SetActive(true);

            if (!string.Equals(CharacterTMP.text, characterName, StringComparison.Ordinal))
            {
                CharacterTMP.text = characterName;
            }

            audio.PlaySound(dialogue, 1f);

            int textIndex = 0;
            while (textIndex < text.Length)
            {
                // If there's input, display all of the text at once
                if (HasInput)
                {
                    // TODO: Stop the audio when an input is given that finishes all the text
                    DialogueTMP.text = text;
                    textIndex = text.Length - 1;
                }
                // Else if the timer is over the time it should take
                // for a character to be shown, show a character
                else if (displayIntervalTimer >= tempDisplayInterval)
                {
                    DialogueTMP.text += ApplyTypography(text, ref textIndex);
                    textIndex++;
                    displayIntervalTimer = 0f;
                }
                else
                {
                    displayIntervalTimer += Time.deltaTime;
                }

                yield return null;
            }

            while (!HasInput)
            {
                yield return null;
            }
        }
        #endregion

        #region Choice

        /// <summary>
        /// Displays a set of choices
        /// </summary>
        /// <param name="options">The options that should be displayed on the screen</param>
        /// <param name="hideDialogue">Set to <see langword="true"/> if the dialogue panel should be hidden when the choices are presented</param>
        /// <param name="displayStyles">A collection of display styles that affect the look of the choices</param>
        public void DisplayChoice(string[] options, bool hideDialogue = true, params TextDisplayStyle[] displayStyles)
        {
            StartCoroutine(DisplayChoiceCoroutine(options, hideDialogue, displayStyles));
        }

        public IEnumerator DisplayChoiceCoroutine(string[] options, bool hideDialogue = true, params TextDisplayStyle[] displayStyles)
        {
            BottomCanvasGroup.gameObject.SetActive(!hideDialogue);
            ChoiceCanvasGroup.gameObject.SetActive(true);

            // Remove previously present choices
            foreach (Transform child in ChoicePanelTransform)
            {
                Destroy(child.gameObject);
            }

            // Creates new buttons in the UI
            for (int i = 0; i < options.Length; i++)
            {
                ChoiceButton button = Instantiate(ChoiceButton, ChoicePanelTransform).GetComponent<ChoiceButton>();
                button.Display(options[i], i, this);
            }

            // If a choice is made, it'll range from 0 -> the amount of options
            while (ChoiceCallback == -1)
            {
                yield return null;
            }

            ChoiceCanvasGroup.gameObject.SetActive(false);
        }
        #endregion

        #region Utility

        /// <summary>
        /// Fades a <see cref="CanvasGroup"/> out with the given time
        /// </summary>
        /// <param name="canvasGroup">The <see cref="CanvasGroup"/> to be hidden</param>
        /// <param name="time">The time it should take to fade the given <see cref="CanvasGroup"/> out</param>
        /// <returns>A Unity <see cref="Coroutine"/></returns>
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

        /// <summary>
        /// Shows a <see cref="CanvasGroup"/> with a fade-in effect
        /// </summary>
        /// <param name="canvasGroup">The <see cref="CanvasGroup"/> to be shown</param>
        /// <param name="time">The time it should take to fade the given <see cref="CanvasGroup"/> in</param>
        /// <returns>A Unity <see cref="Coroutine"/></returns>
        public IEnumerator UnfadeCanvasGroup(CanvasGroup canvasGroup, float time = 1f)
        {
            canvasGroup.gameObject.SetActive(true);

            if (time <= 0f)
            {
                time = 1f;
                Debug.LogWarning("Tried to unfade canvas group with a value for time that's less or equal to zero.");
            }

            while (canvasGroup.alpha != 1f)
            {
                canvasGroup.alpha += Time.deltaTime / time;
                yield return null;
            }
        }

        /// <summary>
        /// Shows the load screen with a fade-in effect
        /// </summary>
        /// <param name="time">The time it should take to fade the load screen in</param>
        /// <param name="hideOtherComponents">Set to <see langword="true"/> if other
        /// components in the UI should fade out while the loading screen is shown</param>
        public void ShowLoadScreen(float time = 1f, bool hideOtherComponents = false)
        {
            if (time <= 0f)
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

        /// <summary>
        /// Hides the load screen with a fade-out effect
        /// </summary>
        /// <param name="time">The time it should take to fade the load screen out</param>
        /// <param name="showOtherComponents">Set to <see langword="true"/> if other
        /// components in the UI should fade in while the loading screen is fading out</param>
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

        /// <summary>
        /// Instantly changes a background to the given <see cref="Sprite"/>
        /// </summary>
        /// <param name="newBackground">The background that should be shown</param>
        public void ChangeBackground(Sprite newBackground)
        {
            BackgroundCanvasGroup.gameObject.SetActive(true);
            BackgroundImage.sprite = newBackground;
        }

        /// <summary>
        /// Changes the background with a crossfade effect
        /// </summary>
        /// <param name="newBackground">The background that should be shown</param>
        /// <param name="transitionTime">The time it should take to crossfade</param>
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

        /// <summary>
        /// Applies a text style to a given <see cref="TextMeshProUGUI"/> component
        /// </summary>
        /// <param name="tmp">The TM_Pro component to apply the text effects to</param>
        /// <param name="displayStyles">The effect that should be applied</param>
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

        /// <summary>
        /// Resets the text style of a given <see cref="TextMeshProUGUI"/> to a default
        /// </summary>
        /// <param name="tmp">The TM_Pro component to apply the text effects to</param>
        private void ResetTMP(TextMeshProUGUI tmp)
        {
            tmp.fontSize = 18f;
            tmp.fontStyle = 0;
            tmp.text = string.Empty;

            tempDisplayInterval = TextDisplayInterval;
        }

        /// <summary>
        /// Removes typography notes marked in a '<' and '>'
        /// </summary>
        /// <param name="text">The text that should be chcked for typography</param>
        /// <param name="textIndex">The index of text that's about to be displayed</param>
        /// <returns>The <paramref name="text"/> without typography notes</returns>
        private string ApplyTypography(string text, ref int textIndex)
        {
            if (text[textIndex] == '<')
            {
                string subString = text.Substring(textIndex);
                int endMark = subString.IndexOf('>');

                if (endMark < 0)
                {
                    return text[textIndex].ToString();
                }

                textIndex += endMark;

                return subString.Substring(0, endMark + 1);
            }
            else return text[textIndex].ToString();
        }

        /// <summary>
        /// Instantly hides all UI components
        /// </summary>
        public void Hide()
        {
            if (BackgroundCanvasGroup != null)
            {
                BackgroundCanvasGroup.gameObject.SetActive(false);
            }

            if (ChoiceCanvasGroup != null)
            {
                ChoiceCanvasGroup.gameObject.SetActive(false);
            }

            if (BottomCanvasGroup != null)
            {
                BottomCanvasGroup.gameObject.SetActive(false);
            }

            if (CharacterNamePlate != null)
            {
                CharacterNamePlate.SetActive(false);
            }
        }

        /// <summary>
        /// Instantly shows all UI components
        /// </summary>
        public void Show()
        {
            if (BackgroundCanvasGroup != null)
            {
                BackgroundCanvasGroup.gameObject.SetActive(true);
            }
        }
    }
}