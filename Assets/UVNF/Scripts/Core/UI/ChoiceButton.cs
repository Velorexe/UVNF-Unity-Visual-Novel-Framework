using TMPro;
using UnityEngine;

namespace UVNF.Core.UI
{
    /// <summary>
    /// The button that a <see cref="Story.Dialogue.ChoiceElement"/> instantiates to present options
    /// </summary>
    public class ChoiceButton : MonoBehaviour
    {
        public TextMeshProUGUI Text;

        private UVNFCanvas CanvasCallback;
        private int _choiceIndex;

        public void Display(string choiceText, int choiceIndex, UVNFCanvas callback)
        {
            CanvasCallback = callback;
            _choiceIndex = choiceIndex;

            Text.text = choiceText;
        }

        public void Chosen()
        {
            CanvasCallback.ChoiceCallback = _choiceIndex;
        }
    }
}