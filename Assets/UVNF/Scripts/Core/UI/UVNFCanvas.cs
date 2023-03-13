using CoroutineManager;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UVNF.Core.UI.Writers;
using UVNF.Core.UI.Writers.Settings;
using UVNF.Entities;
using static UnityEngine.InputSystem.InputAction;

namespace UVNF.Core.UI
{
    /// <summary>
    /// Manages the UI of UVNF. Holds references to the different canvasses, manages
    /// the dialogue, choices and backgrounds that are displayed in the UI of UVNF.
    /// </summary>
    public class UVNFCanvas : MonoBehaviour
    {
        // TODO: Make UVNFManager class with this as a default field
        public UVNFGameResources Resources;

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

        private Task _writerTask = null;

        /// <summary>
        /// The index of the choice that's read from a callback from a choice being displayed
        /// </summary>
        public int ChoiceCallback = -1;

        /// <summary>
        /// Resets the <see cref="ChoiceCallback"/> to -1 (default)
        /// </summary>
        public void ResetChoice() => ChoiceCallback = -1;

        /// <summary>
        /// <see langword="true"/> if there's currently input
        /// </summary>
        private bool HasInput
        {
            get { return _hasInput; }
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

            if (HasInput && _writerTask != null && _writerTask.Running)
            {
                _writerTask.Stop();
                HasInput = false;
            }
        }

        #region Dialogue

        /// <summary>
        /// Displays the given dialogue
        /// </summary>
        /// <param name="text">The dialogue that should be displayed</param>
        /// <param name="displayStyles">A collection of display styles that affect the look of the dialogue</param>
        /// <returns>A Unity <see cref="Coroutine"/></returns>
        public IEnumerator DisplayText(string text, ITextWriter textWriter, TextWriterSettings settings)
        {
            ApplyTextWriterSettingsToTMP(DialogueTMP, settings);
            BottomCanvasGroup.gameObject.SetActive(true);

            DialogueTMP.SetText("");

            // Queue up a Coroutine Task to keep track of if it's finished or not
            _writerTask = new Task(textWriter.Write(DialogueTMP, text, settings.TextDisplaySpeed));

            // Only true if the user clicked before all text is displayed
            _writerTask.Finished += (bool manual) =>
            {
                if (manual)
                {
                    textWriter.WriteInstant(DialogueTMP, text);
                    HasInput = false;
                }
            };

            _writerTask.Start();

            while (_writerTask.Running)
            {
                yield return null;
            }

            _writerTask = null;

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
        public IEnumerator DisplayText(string text, string characterName, ITextWriter textWriter, TextWriterSettings settings, bool useStylesForCharacterField = false)
        {
            if (useStylesForCharacterField)
            {
                ApplyTextWriterSettingsToTMP(CharacterTMP, settings);
            }

            if (!string.Equals(CharacterTMP.text, characterName, StringComparison.Ordinal))
            {
                CharacterTMP.text = characterName;
            }

            CharacterNamePlate.SetActive(!string.IsNullOrEmpty(characterName));

            return DisplayText(text, textWriter, settings);
        }

        /// <summary>
        /// Displays trhe given dialogue with a character nameplate, accompanied by an <see cref="AudioClip"/>
        /// </summary>
        /// <param name="text">The dialogue that should be displayed</param>
        /// <param name="characterName">The name of the character that should be displayed on the nameplate</param>
        /// <param name="dialogue">The audio that should play during the dialogue</param>
        /// <param name="dialogueVolume">The volume at which the dialogue should be played at</param>
        /// <param name="audioManager">The <see cref="AudioManager"/> that should play the <paramref name="dialogue"/></param>
        /// <param name="useStylesForCharacterField"><see langword="true"/> if the nameplate should use the given styles</param>
        /// <param name="displayStyles">A collection of display styles that affect the look of the dialogue</param>
        /// <returns>A Unity <see cref="Coroutine"/></returns>
        public IEnumerator DisplayText(string text, string characterName, ITextWriter textWriter, TextWriterSettings settings, AudioClip dialogue, float dialogueVolume, AudioManager audioManager, bool useStylesForCharacterField = false)
        {
            ApplyTextWriterSettingsToTMP(DialogueTMP, settings);
            if (useStylesForCharacterField)
            {
                ApplyTextWriterSettingsToTMP(CharacterTMP, settings);
            }

            CharacterNamePlate.SetActive(!string.IsNullOrEmpty(characterName));

            BottomCanvasGroup.gameObject.SetActive(true);

            // Queue up a Coroutine Task to keep track of if it's finished or not
            _writerTask = new Task(textWriter.Write(DialogueTMP, text, settings.TextDisplaySpeed));

            _writerTask.Finished += (bool manual) =>
            {
                // Only true if the user clicked before all text is displayed
                if (manual)
                {
                    textWriter.WriteInstant(DialogueTMP, text);
                    audioManager.PauseDialogue();

                    HasInput = false;
                }
            };

            audioManager.PlayDialogue(dialogue, dialogueVolume);
            _writerTask.Start();

            while (_writerTask.Running)
            {
                yield return null;
            }

            _writerTask = null;

            // Wait for input again before proceeding to the next story element
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
        /// Applies the <see cref="TextWriterSettings"/> to the <see cref="TextMeshProUGUI"/>
        /// </summary>
        /// <param name="tmp">The <see cref="TextMeshProUGUI"/> to which the settings should be applied</param>
        /// <param name="settings">The <see cref="TextWriterSettings"/> that should be applied</param>
        private void ApplyTextWriterSettingsToTMP(TextMeshProUGUI tmp, TextWriterSettings settings)
        {
            tmp.fontSize = settings.FontSize;
            tmp.font = settings.Font;

            tmp.color = settings.Color;

            tmp.fontStyle = settings.Styles;

            tmp.ForceMeshUpdate();
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