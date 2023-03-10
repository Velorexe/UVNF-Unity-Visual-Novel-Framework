using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UVNF.Core.UI;
using UVNF.Extensions;
using XNode;

namespace UVNF.Core.Story.Dialogue
{
    /// <summary>
    /// A <see cref="StoryElement"/> that gives the player a choice between a set of options
    /// </summary>
    public class ChoiceElement : StoryElement
    {
        public override string ElementName => "Choice";

        public override StoryElementTypes Type => StoryElementTypes.Story;

        /// <summary>
        /// The choices that player can choose from
        /// </summary>
        public List<string> Choices = new List<string>();

        /// <summary>
        /// Set to <see langword="true"/> if the choices should be shuffled
        /// </summary>
        public bool ShuffleChocies = true;

        /// <summary>
        /// Set to <see langword="true"/> if the dialogue should be hidden while the choices are presented
        /// </summary>
        public bool HideDialogue = false;

#if UNITY_EDITOR
        /// <summary>
        /// Adds a choice to <see cref="Choices"/> and creates a new <see cref="NodePort"/>
        /// </summary>
        public void AddChoice()
        {
            Choices.Add(string.Empty);
            AddDynamicOutput(typeof(NodePort), ConnectionType.Override, TypeConstraint.Inherited, "Choice" + (Choices.Count - 1));
        }

        /// <summary>
        /// Removes a choice from <see cref="Choices"/> and removes the associated <see cref="NodePort"/>
        /// </summary>
        /// <param name="index">The index of the choice that needs to be removed</param>
        public void RemoveChoice(int index)
        {
            Choices.RemoveAt(index);
            RemoveDynamicPort(DynamicPorts.ElementAt(index));
        }
#endif

        public override IEnumerator Execute(UVNFManager managerCallback, UVNFCanvas canvas)
        {
            List<string> choiceList = new List<string>(Choices);

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