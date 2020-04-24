using System;

[Serializable]
public class Variable
{
    public string VariableName = "New Variable";
    public VariableTypes ValueType;

    public float NumberValue = 0f;

    public bool BooleanValue = false;

    public string TextValue = string.Empty;

    public Variable(string name)
    {
        VariableName = name;

        ValueType = VariableTypes.String;
    }
}
