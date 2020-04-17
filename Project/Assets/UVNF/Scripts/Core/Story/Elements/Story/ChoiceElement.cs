using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ChoiceElement : StoryElement
{
    public override string ElementName => "Choice";

    public override Color32 DisplayColor => _displayColor;
    private Color32 _displayColor = new Color32().Story();

    public override StoryElementTypes Type => StoryElementTypes.Story;

    public List<string> Choices = new List<string>(3) { "", "", "" };
    public bool ShuffleChocies = true;
    public bool HideDialogue = false;

    public override void DisplayLayout(Rect layoutRect)
    {
        for (int i = 0; i < Choices.Count; i++)
        {
            GUILayout.Label("Choice " + (i + 1));
            Choices[i] = GUILayout.TextField(Choices[i]);
            if (GUILayout.Button("-"))
            {
                Choices.RemoveAt(i);
                return;
            }
        }

        if (GUILayout.Button("+"))
        {
            Choices.Add(string.Empty);
        }

        ShuffleChocies = GUILayout.Toggle(ShuffleChocies, "Shuffle Choices");
        HideDialogue = GUILayout.Toggle(HideDialogue, "Hide Dialogue");
    }

    public override IEnumerator Execute(GameManager managerCallback, UVNFCanvas canvas)
    {
        List<string> choiceList = Choices;
        if (ShuffleChocies)
        {
            choiceList.Shuffle();
        }

        canvas.DisplayChoice(choiceList.ToArray(), HideDialogue);
        while (canvas.ChoiceCallback == -1) yield return null;
        Debug.Log("Picked a chose.");
    }
}
