using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[Serializable]
public abstract class StoryElement : IComparable
{
    public abstract string ElementName { get; }
    public abstract Color32 DisplayColor { get; }
    public abstract StoryElementTypes Type { get; }

    public abstract IEnumerable Execute();

    public abstract void DisplayLayout();

    public int CompareTo(object obj)
    {
        if (obj == null) return 1;
        if (!(obj is StoryElement)) return 1;
        return string.Compare(ElementName, ((StoryElement)obj).ElementName);
    }
}
