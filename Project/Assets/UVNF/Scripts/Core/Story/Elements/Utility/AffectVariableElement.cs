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

    private MathTypes MathType = MathTypes.Add;
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
                    MathType = (MathTypes)EditorGUILayout.EnumPopup("Action", MathType);
                    Value = EditorGUILayout.FloatField("Value", (float)Value); break;
                case VariableTypes.String:
                    Value = EditorGUILayout.TextField("Value", (string)Value); break;
            }
        }
#endif
    }

    public override IEnumerator Execute(GameManager managerCallback, UVNFCanvas canvas)
    {
        throw new System.NotImplementedException();
    }

    private enum MathTypes
    {
        Add,
        Subtract,
        Divide,
        Multiply
    }
}
