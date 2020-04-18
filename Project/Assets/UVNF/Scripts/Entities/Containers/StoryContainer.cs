using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class StoryContainer : ScriptableObject
{
    public List<StoryElement> StoryElements = new List<StoryElement>();

    public void ConnectStoryElements()
    {
        for (int i = 0; i < StoryElements.Count; i++)
        {
            if (i < StoryElements.Count - 1)
            {
                StoryElements[i].Next = StoryElements[i + 1];
            }
            else
            {
                StoryElements[i].Next = null;
            }
        }
    }
}
