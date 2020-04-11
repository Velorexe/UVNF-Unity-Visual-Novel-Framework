using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryContainer : ScriptableObject
{
    public StoryElement NewElement;

    public List<StoryElement> StoryElements = new List<StoryElement>();

    public void PullNewelement()
    {
        StoryElements.Add(NewElement);
        NewElement = null;
    }
}
