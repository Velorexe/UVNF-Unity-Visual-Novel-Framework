using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UVNF.Core.Story;
using UVNF.Core.Story.Other;
using XNode;
namespace UVNF.Entities.Containers
{
    [CreateAssetMenu()]
    public class StoryGraph : NodeGraph
    {
        public List<StoryElement> StoryElements
        {
            get
            {
                if (_storyElements.Count != nodes.Count)
                {
                    //_storyElements.Clear();
                    //for (int i = 0; i < nodes.Count; i++)
                    //    _storyElements.Add((StoryElement)nodes[i]);

                    RefreshStories();
                }
                return _storyElements;
            }
        }
        private List<StoryElement> _storyElements = new List<StoryElement>();

        public string[] StoryNames = new string[] { };
        private List<StoryElement>[] _shortStories = new List<StoryElement>[] { };

        public void RefreshStories()
        {
            Node[] startNodesArray = nodes.Where(x => x.GetType() == typeof(StartElement)).ToArray();
            StartElement[] startNodes = new StartElement[startNodesArray.Length];

            for (int i = 0; i < startNodes.Length; i++)
            {
                startNodes[i] = startNodesArray[i] as StartElement;
            }

            StoryNames = startNodes.Select(x => x.StoryName).ToArray();

            _shortStories = new List<StoryElement>[startNodes.Length];
            for (int i = 0; i < _shortStories.Length; i++)
            {
                _shortStories[i] = new List<StoryElement>();
                _shortStories[i].Add(startNodes[i]);

                StartElement currentStartNode = startNodes[i];
                StoryElement currentNode = startNodes[i].GetOutputPort("NextNode").GetOutputValue() as StoryElement;
                while (currentNode != null && currentNode.GetOutputPort("NextNode").IsConnected && currentNode.GetOutputPort("NextNode").GetOutputValue().GetType() != typeof(StartElement))
                {
                    _shortStories[i].Add(currentNode);
                    currentNode = currentNode.GetOutputPort("NextNode").GetOutputValue() as StoryElement;
                }
                if (currentNode != null && currentNode.GetType() != typeof(StartElement))
                {
                    _shortStories[i].Add(currentNode);
                }
            }
        }

        public List<StoryElement> ShortStory(int storyIndex)
        {
            if (storyIndex < _shortStories.Length && storyIndex > -1)
            {
                return _shortStories[storyIndex];
            }

            return new List<StoryElement>();
        }

        public List<StoryElement> GetRootStory()
        {
            RefreshStories();
            return _shortStories.Where(x => (x[0] as StartElement).IsRoot).First().ToList();
        }

        public void ConnectStoryElements()
        {
            nodes = nodes.Where(x => x != null).ToList();
            foreach (Node element in nodes)
            {
                (element as StoryElement).Connect();
            }
        }
    }
}