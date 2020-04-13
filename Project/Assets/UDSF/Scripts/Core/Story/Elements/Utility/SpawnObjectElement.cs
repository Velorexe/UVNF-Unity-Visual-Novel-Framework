using System.Collections;
using UnityEngine;
using UnityEditor;

public class SpawnObjectElement : StoryElement
{
    public override string ElementName => "Spawn Object";
    public override Color32 DisplayColor => new Color32(0xB3, 0xBD, 0xED, 0xff);
    public override StoryElementTypes Type => StoryElementTypes.Utility;

    public GameObject ObjectToSpawn;

    public override void DisplayLayout()
    {
#if UNITY_EDITOR
        GUILayout.Label("Object To Spawn", EditorStyles.boldLabel);
        ObjectToSpawn = EditorGUILayout.ObjectField(ObjectToSpawn, typeof(GameObject), false) as GameObject;
#endif
    }

    public override IEnumerator Execute(GameManager managerCallback, UDSFCanvas canvas)
    {
        return null;
    }
}
