using System.Collections;
using UnityEngine;
using UVNF.Core.UI;

namespace UVNF.Core.Story.Utility
{
    public class SpawnObjectElement : StoryElement
    {
        public override string ElementName => "Spawn Object";

        public override StoryElementTypes Type => StoryElementTypes.Utility;

        public GameObject ObjectToSpawn;
        private GameObject _spawnedObject;

        public override IEnumerator Execute(UVNFManager managerCallback, UVNFCanvas canvas)
        {
            if (ObjectToSpawn != null)
            {
                _spawnedObject = Instantiate(ObjectToSpawn);
            }
            else
            {
                Debug.LogError("Spawn Object Element doesn't contain an element to Instantiate.");
            }

            return null;
        }
    }
}