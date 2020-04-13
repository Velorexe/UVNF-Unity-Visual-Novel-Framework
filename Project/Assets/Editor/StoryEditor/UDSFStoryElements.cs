using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class UDSFStoryElements : EditorWindow
{
    [MenuItem("UDSF/Story Elements")]
    public static void Init()
    {
        UDSFStoryElements window = GetWindow<UDSFStoryElements>();
        window.Show();
    }

    public void OnGUI()
    {
        
    }
}
