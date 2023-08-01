using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UVNF.Core.UI;
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
        [Output(
            dynamicPortList = true,
            connectionType = ConnectionType.Override)]
        public string[] Choices = Array.Empty<string>();

        /// <summary>
        /// Set to <see langword="true"/> if the dialogue should be hidden while the choices are presented
        /// </summary>
        public bool HideDialogue = false;

        public override IEnumerator Execute(UVNFManager managerCallback, UVNFCanvas canvas)
        {
            List<string> choiceList = new List<string>(Choices);

            canvas.DisplayChoice(choiceList.ToArray(), HideDialogue);

            while (canvas.ChoiceCallback == -1)
            {
                yield return null;
            }

            if (DynamicPorts.ElementAt(canvas.ChoiceCallback).IsConnected)
            {
                NodePort nodePort = DynamicPorts.ElementAt(canvas.ChoiceCallback);
                int choice = canvas.ChoiceCallback;

                canvas.ResetChoice();
                managerCallback.AdvanceStoryGraph(DynamicPorts.ElementAt(choice).Connection.node as StoryElement);
            }
        }
    }
}