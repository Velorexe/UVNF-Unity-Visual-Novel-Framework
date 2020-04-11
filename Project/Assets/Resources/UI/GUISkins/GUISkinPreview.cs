using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUISkinPreview : MonoBehaviour
{
    public GUISkin guiSkin;
    public string Text;

    private void OnGUI()
    {
        GUILayout.Button(Text, guiSkin.button);
    }
}
