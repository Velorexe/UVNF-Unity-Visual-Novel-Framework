using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class UVNFStoryElements : EditorWindow
{
    [MenuItem("UDSF/Story Elements")]
    public static void Init()
    {
        UVNFStoryElements window = GetWindow<UVNFStoryElements>();
        window.Show();
    }

    public void OnGUI()
    {
        
    }
}
