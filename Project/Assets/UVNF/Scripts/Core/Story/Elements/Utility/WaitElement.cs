using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class WaitElement : StoryElement
{
    public override string ElementName => "Wait";

    public override Color32 DisplayColor => _displayColor;
    private Color32 _displayColor = new Color32().Utility();

    public override StoryElementTypes Type => StoryElementTypes.Utility;

    public float WaitTime= 1f;

    public override void DisplayLayout(Rect layoutRect)
    {
#if UNITY_EDITOR
        WaitTime = EditorGUILayout.FloatField("Wait Time", WaitTime);
        WaitTime = EditorGUILayout.Slider("Wait Time", WaitTime, 0.1f, WaitTime > 10f ? WaitTime : 10f);
#endif
    }

    public override IEnumerator Execute(GameManager managerCallback, UVNFCanvas canvas)
    {
        float currentTime = 0f;
        while (currentTime < WaitTime)
        {
            currentTime += Time.deltaTime;
            yield return null;
        }
    }
}
