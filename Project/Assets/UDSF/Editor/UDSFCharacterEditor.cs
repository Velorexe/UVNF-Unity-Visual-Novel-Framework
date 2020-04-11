using UnityEditor;
using UnityEngine;

using Entities;

public class UDSFCharacterEditor : EditorWindowExtended
{
    public CharacterDatabase PersistentCharacterDatabase;
    public Character CurrentCharacter = null;

    [MenuItem("UDSF/Characters")]
    public static void Init()
    {
        UDSFCharacterEditor editorWindow = (UDSFCharacterEditor)GetWindow(typeof(UDSFCharacterEditor));
        editorWindow.Show();

        editorWindow.PersistentCharacterDatabase = DatabaseManager.GetCharacterDatabase();
    }

    public void OnGUI()
    {
        BeginHorizontal();
        {
            DrawItemList("Characters", PersistentCharacterDatabase.GetCharacterNames(), Height(position.height - 6), Width(200)); 
            BoxVertical();
            {
                EditorGUILayout.LabelField(PersistentCharacterDatabase.GetCharacterNameAtIndex(SelectedIndex));
            }
            EndVertical();
        }
    }
}
