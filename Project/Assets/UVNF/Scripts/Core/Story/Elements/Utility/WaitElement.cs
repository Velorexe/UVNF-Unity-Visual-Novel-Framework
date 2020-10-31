using System.Collections;
using UnityEngine;
using UnityEditor;
using UVNF.Core.UI;
using UVNF.Extensions;

namespace UVNF.Core.Story.Utility
{
    public class WaitElement : StoryElement
    {
        public override string ElementName => "Wait";

        public override Color32 DisplayColor => _displayColor;
        private Color32 _displayColor = new Color32().Utility();

        public override StoryElementTypes Type => StoryElementTypes.Utility;

        public float WaitTime = 1f;

        public override void DisplayLayout(Rect layoutRect, GUIStyle label)
        {
#if UNITY_EDITOR
            WaitTime = EditorGUILayout.FloatField("Wait Time", WaitTime);
            WaitTime = EditorGUILayout.Slider("Wait Time", WaitTime, 0.1f, WaitTime > 10f ? WaitTime : 10f);
#endif
        }

        public override IEnumerator Execute(UVNFManager managerCallback, UVNFCanvas canvas)
        {
            float currentTime = 0f;
            while (currentTime < WaitTime)
            {
                currentTime += Time.deltaTime;
                yield return null;
            }
        }
    }
}