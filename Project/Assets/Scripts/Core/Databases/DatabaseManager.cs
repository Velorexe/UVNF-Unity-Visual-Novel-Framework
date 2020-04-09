using UnityEngine;
using UnityEditor;
using System.IO;

public static class DatabaseManager
{
    private static CharacterDatabase _characterDatabase;

    public static CharacterDatabase GetCharacterDatabase()
    {
        if (!File.Exists(Application.dataPath + @"Scripts/Core/Databases/Instances/CharacterDatabase.asset"))
        {
            CharacterDatabase db = ScriptableObject.CreateInstance(typeof(CharacterDatabase)) as CharacterDatabase;
            for (int i = 0; i < 10; i++)
            {
            db.AddCharacter();

            }
            AssetDatabase.CreateAsset(db, "Assets/Scripts/Core/Databases/Instances/CharacterDatabase.asset");
            AssetDatabase.SaveAssets();
            _characterDatabase = AssetDatabase.LoadAssetAtPath(@"Assets/Scripts/Core/Databases/Instances/CharacterDatabase.asset", typeof(CharacterDatabase)) as CharacterDatabase;
        }
        return _characterDatabase;
    }
}
