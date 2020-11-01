using System.Collections;
using UnityEngine;
using UnityEditor;
using UVNF.Core.UI;
using UVNF.Extensions;

namespace UVNF.Core.Story.Utility
{
    public class SpawnObjectElement : StoryElement
    {
        public override string ElementName => "Spawn Object";

        public override Color32 DisplayColor => _displayColor;
        private Color32 _displayColor = new Color32().Utility();

        public override StoryElementTypes Type => StoryElementTypes.Utility;

        public GameObject ObjectToSpawn;
        private GameObject _spawnedObject;

#if UNITY_EDITOR
        public override void DisplayLayout(Rect layoutRect, GUIStyle label)
        {
            GUILayout.Label("Object To Spawn", EditorStyles.boldLabel);
            ObjectToSpawn = EditorGUILayout.ObjectField(ObjectToSpawn, typeof(GameObject), false) as GameObject;
        }
#endif

        public override IEnumerator Execute(UVNFManager managerCallback, UVNFCanvas canvas)
        {
            if (ObjectToSpawn != null)
                _spawnedObject = Instantiate(ObjectToSpawn);
            else Debug.LogError("Spawn Object Element doesn't contain an element to Instantiate.");
            return null;
        }
    }
}