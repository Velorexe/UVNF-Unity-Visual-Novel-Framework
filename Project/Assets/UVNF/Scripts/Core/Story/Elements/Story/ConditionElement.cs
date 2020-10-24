using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using XNode;
using UVNF.Core.UI;
using UVNF.Entities.Containers.Variables;
using UVNF.Extensions;

namespace UVNF.Core.Story.Dialogue
{
    public class ConditionElement : StoryElement
    {
        public override string ElementName => "Condition";

        public override Color32 DisplayColor => _displayColor;
        private Color32 _displayColor = new Color32().Story();

        public override StoryElementTypes Type => StoryElementTypes.Story;

        [HideInInspector]
        [Output(ShowBackingValue.Never, ConnectionType.Override)] public NodePort ConditionFails;

        public VariableManager Variables;
        public int VariableIndex = 0;

        public float NumberValue = 0f;
        public string TextValue = string.Empty;
        public bool BooleanValue = false;

        public override void DisplayLayout(Rect layoutRect, GUIStyle label)
        {
#if UNITY_EDITOR
            Variables = EditorGUILayout.ObjectField("Variables", Variables, typeof(VariableManager), false) as VariableManager;
            if (Variables != null && Variables.Variables.Count > 0)
            {
                VariableIndex = EditorGUILayout.Popup(VariableIndex, Variables.VariableNames());
            }
#endif
        }

        public override IEnumerator Execute(GameManager managerCallback, UVNFCanvas canvas)
        {
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
                managerCallback.AdvanceStory(GetOutputPort("NextNode").Connection.node as StoryElement);
            else
                managerCallback.AdvanceStory(GetOutputPort("ConditionFails").Connection.node as StoryElement);
            yield return null;
        }
    }
}