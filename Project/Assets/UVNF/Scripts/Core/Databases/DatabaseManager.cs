using UnityEngine;
using UnityEditor;
using System.IO;

public static class DatabaseManager
{
    private static CharacterDatabase _characterDatabase;

    public static CharacterDatabase GetCharacterDatabase()
    {
#if UNITY_STANDALONE
        _characterDatabase = Resources.Load<CharacterDatabase>("Databases/CharacterDatabase.asset");
#endif
#if UNITY_EDITOR
        if (!File.Exists(Application.dataPath + @"/Resources/Databases/CharacterDatabase.asset"))
        {
            CharacterDatabase db = ScriptableObject.CreateInstance(typeof(CharacterDatabase)) as CharacterDatabase;
            for (int i = 0; i < 10; i++)
            {
            db.AddCharacter();

            }
            AssetDatabase.CreateAsset(db, @"Assets/Resources/Databases/CharacterDatabase.asset");
            AssetDatabase.SaveAssets();
        }
        _characterDatabase = AssetDatabase.LoadAssetAtPath(@"Assets/Resources/Databases/CharacterDatabase.asset", typeof(CharacterDatabase)) as CharacterDatabase;
#endif
        return _characterDatabase;
    }
}
