using System.Collections;
using UVNF.Core.UI;
using UVNF.Entities.Containers.Variables;

namespace UVNF.Core.Story.Utility
{
    public class AffectVariableElement : StoryElement
    {
        public override string ElementName => "Affect Variable";

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

        public override IEnumerator Execute(UVNFManager managerCallback, UVNFCanvas canvas)
        {
            //TODO: Refactor variables, they're confusing in their current state
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