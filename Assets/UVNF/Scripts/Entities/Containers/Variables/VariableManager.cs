using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UVNF.Core.Serialization;

namespace UVNF.Entities.Containers.Variables
{
    [CreateAssetMenu]
    public class VariableManager : ScriptableObject, ISerializationCallbackReceiver
    {
        public List<Variable> Variables = new List<Variable>();

        public List<SerializedData> VariableValues = new List<SerializedData>();

        public void AddVariable()
        {
            Variables.Add(new Variable("New Variable"));
        }

        public string[] VariableNames()
        {
            return Variables.Select(x => x.VariableName).ToArray();
        }

        #region Serialization
        public void OnAfterDeserialize()
        {
            Variables.Clear();
            for (int i = 0; i < VariableValues.Count; i++)
                Variables.Add(SerializedData.Deserialize(VariableValues[i]) as Variable);
        }

        public void OnBeforeSerialize()
        {
            VariableValues.Clear();
            for (int i = 0; i < Variables.Count; i++)
                VariableValues.Add(SerializedData.Serialize(Variables[i]));
        }
        #endregion
    }
}