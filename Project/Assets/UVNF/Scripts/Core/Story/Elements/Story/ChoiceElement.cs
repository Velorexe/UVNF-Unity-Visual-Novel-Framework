using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using XNode;

public class ChoiceElement : StoryElement
{
    public override string ElementName => "Choice";

    public override Color32 DisplayColor => _displayColor;
    private Color32 _displayColor = new Color32().Story();

    public override StoryElementTypes Type => StoryElementTypes.Story;

    public List<string> Choices = new List<string>();

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
                RemoveChoice(i);
                return;
            }
        }

        if (GUILayout.Button("+"))
            AddChoice();

        ShuffleChocies = GUILayout.Toggle(ShuffleChocies, "Shuffle Choices");
        HideDialogue = GUILayout.Toggle(HideDialogue, "Hide Dialogue");
    }

    public void AddChoice()
    {
        Choices.Add(string.Empty);
        AddDynamicOutput(typeof(NodePort), ConnectionType.Override, TypeConstraint.Inherited, "Choice" + (Choices.Count - 1));
    }

    public void RemoveChoice(int index)
    {
        Choices.RemoveAt(index);
        RemoveDynamicPort(DynamicPorts.ElementAt(index));
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

        if (DynamicPorts.ElementAt(canvas.ChoiceCallback).IsConnected)
            managerCallback.AdvanceStory(DynamicPorts.ElementAt(canvas.ChoiceCallback).Connection.node as StoryElement);
    }
}
