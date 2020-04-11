using System.Collections;
using UnityEngine;
using UnityEditor;

public class SpawnObjectElement : StoryElement
{
    public override string ElementName { get { return "Spawn Object"; } }
    public override Color32 DisplayColor { get { return new Color32(0xB3, 0xBD, 0xED, 0xff); } }
    public override StoryElementTypes Type { get { return StoryElementTypes.Utility; } }

    public GameObject ObjectToSpawn;

    public override void DisplayLayout()
    {
        GUILayout.Label("Object To Spawn", EditorStyles.boldLabel);
        ObjectToSpawn = EditorGUILayout.ObjectField(ObjectToSpawn, typeof(GameObject), false) as GameObject;
    }

    public override IEnumerable Execute(GameManager managerCallback, UDSFCanvas canvas)
    {
        return null;
    }
}
