using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public abstract class StoryElement : ScriptableObject, IComparable
{
    public abstract string ElementName { get; }
    public abstract Color32 DisplayColor { get; }
    public abstract StoryElementTypes Type { get; }

    public StoryElement Next;

    public abstract IEnumerable Execute(GameManager managerCallback, UDSFCanvas canvas);

    public abstract void DisplayLayout();

    public int CompareTo(object obj)
    {
        if (obj == null) return 1;
        if (!(obj is StoryElement)) return 1;
        return string.Compare(ElementName, ((StoryElement)obj).ElementName);
    }
}
