using System.Collections;
using UnityEngine;
using UnityEditor;
using UVNF.Core.UI;
using UVNF.Entities.Containers.Variables;
using UVNF.Extensions;

namespace UVNF.Core.Story.Utility
{
    public class AffectVariableElement : StoryElement
    {
        public override string ElementName => "Affect Variable";

        public override Color32 DisplayColor => _displayColor;
        private Color32 _displayColor = new Color32().Utility();

        public override StoryElementTypes Type => StoryElementTypes.Utility;

        public VariableManager Variables;

        public int VariableIndex = 0;
        private int previousIndex = 0;

        private VariableTypes previousType = VariableTypes.Number;

        private MathAffectTypes MathType = MathAffectTypes.Add;
        private StringAffectTypes StringType = StringAffectTypes.Replace;

        public float NumberValue = 0f;

        public bool BooleanValue = false;

        public string TextValue = string.Empty;

        private string[] booleanOptions = new string[] { "False", "True" };

        public override void DisplayLayout(Rect layoutRect, GUIStyle label)
        {
#if UNITY_EDITOR
            Variables = EditorGUILayout.ObjectField("Variables", Variables, typeof(VariableManager), false) as VariableManager;
            if (Variables != null && Variables.Variables.Count > 0)
            {
                VariableIndex = EditorGUILayout.Popup("Variable", VariableIndex, Variables.VariableNames());

                switch (Variables.Variables[VariableIndex].ValueType)
                {
                    case VariableTypes.Number:
                        MathType = (MathAffectTypes)EditorGUILayout.EnumPopup("Action", MathType);
                        NumberValue = EditorGUILayout.FloatField("Value", NumberValue); break;
                    case VariableTypes.String:
                        StringType = (StringAffectTypes)EditorGUILayout.EnumPopup("Action", StringType);
                        TextValue = EditorGUILayout.TextField("Value", TextValue); break;
                    case VariableTypes.Boolean:
                        BooleanValue = System.Convert.ToBoolean(EditorGUILayout.Popup("Value", System.Convert.ToInt32(BooleanValue), booleanOptions)); break;
                }
            }
#endif
        }

        public override IEnumerator Execute(UVNFManager managerCallback, UVNFCanvas canvas)
        {
            switch (Variables.Variables[VariableIndex].ValueType)
            {
                case VariableTypes.Number:
                    switch (MathType)
                    {
                        case MathAffectTypes.Add:
                            Variables.Variables[VariableIndex].NumberValue += NumberValue; break;
                        case MathAffectTypes.Subtract:
                            Variables.Variables[VariableIndex].NumberValue -= NumberValue; break;
                        case MathAffectTypes.Divide:
                            Variables.Variables[VariableIndex].NumberValue /= NumberValue; break;
                        case MathAffectTypes.Multiply:
                            Variables.Variables[VariableIndex].NumberValue *= NumberValue; break;
                    }
                    break;
                case VariableTypes.String:
                    switch (StringType)
                    {
                        case StringAffectTypes.Add:
                            Variables.Variables[VariableIndex].TextValue += TextValue; break;
                        case StringAffectTypes.Remove:
                            Variables.Variables[VariableIndex].TextValue = Variables.Variables[VariableIndex].TextValue.Replace(TextValue, ""); break;
                        case StringAffectTypes.Replace:
                            Variables.Variables[VariableIndex].TextValue = TextValue; break;
                    }
                    break;
                case VariableTypes.Boolean:
                    Variables.Variables[VariableIndex].BooleanValue = BooleanValue; break;
            }
            return null;
        }

        public enum MathAffectTypes
        {
            Add,
            Subtract,
            Divide,
            Multiply
        }

        public enum StringAffectTypes
        {
            Replace,
            Add,
            Remove
        }
    }
}