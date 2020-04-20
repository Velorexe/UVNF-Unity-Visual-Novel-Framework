using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using XNode;

[CreateAssetMenu()]
public class StoryGraph : NodeGraph
{
    public List<StoryElement> StoryElements
    {
        get
        {
            if (_storyElements.Count != nodes.Count)
            {
                _storyElements.Clear();
                for (int i = 0; i < nodes.Count; i++)
                    _storyElements.Add((StoryElement)nodes[i]);
            }
            return _storyElements;
        }
    }
    private List<StoryElement> _storyElements = new List<StoryElement>();

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
