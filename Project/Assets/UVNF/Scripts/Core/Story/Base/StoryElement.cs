using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using XNode;

public abstract class StoryElement : ScriptableObject, IComparable
{
    public abstract string ElementName { get; }
    public abstract Color32 DisplayColor { get; }
    public abstract StoryElementTypes Type { get; }

    [HideInInspector]
    public bool Active = false;
    [HideInInspector]
    public StoryElement Next;

    public abstract IEnumerator Execute(GameManager managerCallback, UVNFCanvas canvas);

    public abstract void DisplayLayout();

    public int CompareTo(object obj)
    {
        if (obj == null) return 1;
        if (!(obj is StoryElement)) return 1;
        return string.Compare(ElementName, ((StoryElement)obj).ElementName);
    }
}
