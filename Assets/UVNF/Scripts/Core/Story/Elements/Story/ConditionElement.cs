using System.Collections;
using UnityEngine;
using UVNF.Core.UI;
using UVNF.Entities.Containers.Variables;
using XNode;

namespace UVNF.Core.Story.Dialogue
{
    public class ConditionElement : StoryElement
    {
        public override string ElementName => "Condition";

        public override StoryElementTypes Type => StoryElementTypes.Story;

        [HideInInspector]
        [Output(ShowBackingValue.Never, ConnectionType.Override)] public NodePort ConditionFails;

        public VariableManager Variables;
        public int VariableIndex = 0;

        public float NumberValue = 0f;

        public string TextValue = string.Empty;

        public bool BooleanValue = false;

        public override IEnumerator Execute(UVNFManager managerCallback, UVNFCanvas canvas)
        {
            // TODO: Refactor, variables are confusing in their current state

            bool conditionTrue = false;
            switch (Variables.Variables[VariableIndex].ValueType)
            {
                case VariableTypes.Boolean:
                    conditionTrue = BooleanValue == Variables.Variables[VariableIndex].BooleanValue; break;
                case VariableTypes.Number:
                    conditionTrue = NumberValue >= Variables.Variables[VariableIndex].NumberValue; break;
                case VariableTypes.String:
                    conditionTrue = TextValue == Variables.Variables[VariableIndex].TextValue; break;
            }

            if (conditionTrue)
            {
                managerCallback.AdvanceStoryGraph(GetOutputPort("NextNode").Connection.node as StoryElement);
            }
            else
            {
                managerCallback.AdvanceStoryGraph(GetOutputPort("ConditionFails").Connection.node as StoryElement);
            }

            yield return null;
        }
    }
}