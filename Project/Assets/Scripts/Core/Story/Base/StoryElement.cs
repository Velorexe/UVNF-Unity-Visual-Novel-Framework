using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public abstract class StoryElement
{
    public abstract string ElementName { get; }
    public abstract Color32 DisplayColor { get; }

    public abstract void Execute();

    public abstract void DisplayLayout();
}
