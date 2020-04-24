using System;

[Serializable]
public class Variable
{
    public string VariableName = "New Variable";

    public VariableTypes ValueType
    {
        get { return _valueType; }
        set 
        {
            switch (value)
            {
                case VariableTypes.Number:
                    Value = 0f; break;
                case VariableTypes.String:
                    Value = string.Empty; break;
            }
            _valueType = value;
        }
    }
    private VariableTypes _valueType;
    public object Value;

    public Variable(string name)
    {
        VariableName = name;

        ValueType = VariableTypes.String;
        Value = string.Empty;
    }
}
