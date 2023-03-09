using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UVNF.Core.UI;
using UVNF.Extensions;
using XNode;

namespace UVNF.Core.Story.Dialogue
{
    public class ChoiceElement : StoryElement
    {
        public override string ElementName => "Choice";

        public override StoryElementTypes Type => StoryElementTypes.Story;

        public List<string> Choices = new List<string>();

        public bool ShuffleChocies = true;
        public bool HideDialogue = false;

#if UNITY_EDITOR
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
#endif

        public override IEnumerator Execute(UVNFManager managerCallback, UVNFCanvas canvas)
        {
            List<string> choiceList = Choices;

            if (ShuffleChocies)
            {
                choiceList.Shuffle();
            }

            canvas.DisplayChoice(choiceList.ToArray(), HideDialogue);

            while (canvas.ChoiceCallback == -1)
            {
                yield return null;
            }

            if (DynamicPorts.ElementAt(canvas.ChoiceCallback).IsConnected)
            {
                int choice = canvas.ChoiceCallback;
                canvas.ResetChoice();
                managerCallback.AdvanceStoryGraph(DynamicPorts.ElementAt(choice).Connection.node as StoryElement);
            }
        }
    }
}