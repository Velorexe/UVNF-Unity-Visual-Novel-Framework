using UnityEngine;
using TMPro;

public class ChoiceButton : MonoBehaviour
{
    public TextMeshProUGUI Text;
    
    private UDSFCanvas CanvasCallback;
    private int ChoiceIndex;
    
    public void Display(string choiceText, int choiceIndex, UDSFCanvas callback)
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
