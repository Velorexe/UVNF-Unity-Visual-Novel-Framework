using System.Collections;
using UnityEngine;
using UnityEditor;

public class SpawnObjectElement : StoryElement
{
    public override string ElementName => "Spawn Object";
    public override Color32 DisplayColor => UDSFSettings.Settings.UtilityElementColor;
    public override StoryElementTypes Type => StoryElementTypes.Utility;

    public GameObject ObjectToSpawn;

    public override void DisplayLayout()
    {
        GUILayout.Label("Object To Spawn", EditorStyles.boldLabel);
        ObjectToSpawn = EditorGUILayout.ObjectField(ObjectToSpawn, typeof(GameObject), false) as GameObject;
    }

    public override IEnumerator Execute(GameManager managerCallback, UDSFCanvas canvas)
    {
        return null;
    }
}
