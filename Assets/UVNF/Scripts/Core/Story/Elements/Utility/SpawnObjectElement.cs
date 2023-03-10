using System.Collections;
using UnityEngine;
using UVNF.Core.UI;

namespace UVNF.Core.Story.Utility
{
    /// <summary>
    /// A <see cref="StoryElement"/> that spawns a <see cref="GameObject"/> inside the scene
    /// </summary>
    public class SpawnObjectElement : StoryElement
    {
        public override string ElementName => "Spawn Object";

        public override StoryElementTypes Type => StoryElementTypes.Utility;

        /// <summary>
        /// The <see cref="GameObject"/> that should be instantiated inside the scene
        /// </summary>
        public GameObject ObjectToSpawn;

        /// <summary>
        /// A reference to the instantiated <see cref="GameObject"/>
        /// </summary>
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