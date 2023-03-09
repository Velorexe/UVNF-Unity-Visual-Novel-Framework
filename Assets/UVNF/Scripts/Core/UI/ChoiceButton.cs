using UnityEngine;
using TMPro;

namespace UVNF.Core.UI
{
    public class ChoiceButton : MonoBehaviour
    {
        public TextMeshProUGUI Text;

        private UVNFCanvas CanvasCallback;
        private int ChoiceIndex;

        public void Display(string choiceText, int choiceIndex, UVNFCanvas callback)
        {
            CanvasCallback = callback;
            ChoiceIndex = choiceIndex;

            Text.text = choiceText;
        }

        public void Chosen()
        {
            CanvasCallback.ChoiceCallback = ChoiceIndex;
        }
    }
}