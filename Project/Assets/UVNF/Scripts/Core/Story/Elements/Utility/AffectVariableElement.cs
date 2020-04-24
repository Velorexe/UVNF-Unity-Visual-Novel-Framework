using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class AffectVariableElement : StoryElement
{
    public override string ElementName => "Affect Variable";

    public override Color32 DisplayColor => _displayColor;
    private Color32 _displayColor = new Color32().Utility();

    public override StoryElementTypes Type => StoryElementTypes.Utility;

    public VariableManager Variables;

    public int VariableIndex = -1;
    private int previousIndex = -1;

    private VariableTypes previousType = VariableTypes.Number;

    private MathAffectTypes MathType = MathAffectTypes.Add;
    private StringAffectTypes StringType = StringAffectTypes.Replace;

    public object Value;

    public override void DisplayLayout(Rect layoutRect)
    {
#if UNITY_EDITOR
        Variables = EditorGUILayout.ObjectField("Variables", Variables, typeof(VariableManager), false) as VariableManager;
        if (Variables != null && Variables.Variables.Count > 0)
        {
            VariableIndex = EditorGUILayout.Popup("Variable", VariableIndex, Variables.VariableNames());
            if (VariableIndex != previousIndex)
            {
                previousIndex = VariableIndex;
                Value = Variables.Variables[VariableIndex].Value;
            }

            if(previousType != Variables.Variables[VariableIndex].ValueType)
            {
                previousType = Variables.Variables[VariableIndex].ValueType;
                Value = Variables.Variables[VariableIndex].Value;
            }

            switch (Variables.Variables[VariableIndex].ValueType)
            {
                case VariableTypes.Number:
                    MathType = (MathAffectTypes)EditorGUILayout.EnumPopup("Action", MathType);
                    Value = EditorGUILayout.FloatField("Value", (float)Value); break;
                case VariableTypes.String:
                    StringType = (StringAffectTypes)EditorGUILayout.EnumPopup("Action", StringType);
                    Value = EditorGUILayout.TextField("Value", (string)Value); break;
            }
        }
#endif
    }

    public override IEnumerator Execute(GameManager managerCallback, UVNFCanvas canvas)
    {
        switch (Variables.Variables[VariableIndex].ValueType)
        {
            case VariableTypes.Number:
                switch (MathType)
                {
                    case MathAffectTypes.Add:
                        Variables.Variables[VariableIndex].Value = (float)Variables.Variables[VariableIndex].Value + (float)Value; break;
                    case MathAffectTypes.Subtract:
                        Variables.Variables[VariableIndex].Value = (float)Variables.Variables[VariableIndex].Value - (float)Value; break;
                    case MathAffectTypes.Divide:
                        Variables.Variables[VariableIndex].Value = (float)Variables.Variables[VariableIndex].Value / (float)Value; break;
                    case MathAffectTypes.Multiply:
                        Variables.Variables[VariableIndex].Value = (float)Variables.Variables[VariableIndex].Value * (float)Value; break;
                }
                break;
            case VariableTypes.String:
                switch (StringType)
                {
                    case StringAffectTypes.Add:
                        Variables.Variables[VariableIndex].Value = (string)Variables.Variables[VariableIndex].Value + (string)Value; break;
                    case StringAffectTypes.Remove:
                        Variables.Variables[VariableIndex].Value = ((string)Variables.Variables[VariableIndex].Value).Replace((string)Value, ""); break;
                    case StringAffectTypes.Replace:
                        Variables.Variables[VariableIndex].Value = (string)Value; break;
                }
                break;
        }
        return null;
    }

    private enum MathAffectTypes
    {
        Add,
        Subtract,
        Divide,
        Multiply
    }

    private enum StringAffectTypes
    {
        Replace,
        Add,
        Remove
    }
}
