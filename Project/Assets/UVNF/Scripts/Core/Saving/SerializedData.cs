using System;
using UnityEngine;

[Serializable]
public class SerializedData
{
    public string Type;
    public string Data;

    public static SerializedData Serialize(object obj)
    {
        return new SerializedData()
        {
            Type = obj.GetType().AssemblyQualifiedName,
            Data = JsonUtility.ToJson(obj)
        };
    }

    public static object Deserialize(SerializedData sd)
    {
        Type objectType = System.Type.GetType(sd.Type);
        return JsonUtility.FromJson(sd.Data, objectType);
    }
}
