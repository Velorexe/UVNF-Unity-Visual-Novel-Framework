using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using CoroutineManager;

using UVNF.Core.UI;
using UVNF.Core.Story;

using UVNF.Entities.Containers;
using UVNF.Entities.Containers.Variables;

namespace UVNF.Core
{
    public class UVNFManager : MonoBehaviour
    {
        [Header("UDSF Components")]
        public UVNFCanvas Canvas;
        public AudioManager AudioManager;
        public CanvasCharacterManager CharacterManager;

        [Header("Variables")]
        public VariableManager Variables;

        private UVNFStoryManager _currentStoryManager;

        private Queue<StoryGraph> _graphQueue = new Queue<StoryGraph>();

        /// <summary>
        /// Starts a provided StoryGraph
        /// </summary>
        /// <param name="graph"></param>
        /// <returns>True if the Story is started directly. False if the provided Graph is Queued.</returns>
        public bool StartStory(StoryGraph graph)
        {
            if (_currentStoryManager == null)
            {
                Canvas.Show();
                _currentStoryManager = new UVNFStoryManager(graph, this, Canvas, FinishStory);
                return true;
            }

            QueueStory(graph);
            return false;
        }

        private void QueueStory(StoryGraph graph)
        {
            _graphQueue.Enqueue(graph);
        }

        public void StartSubStory(StoryGraph subGraph)
        {
            _currentStoryManager.CreateSubStory(subGraph, this, Canvas);
        }

        public void AdvanceStoryGraph(StoryElement element)
        {
            _currentStoryManager.AdvanceStory(element);
        }

        public void AdvanceStoryGraph(StoryElement element, bool continueInBackground)
        {
            _currentStoryManager.AdvanceStory(element, continueInBackground);
        }

        private void FinishStory()
        {
            _currentStoryManager = null;
            //Story still left in the Queue
            if (_graphQueue.Count > 0)
                StartStory(_graphQueue.Dequeue());
            //Story if finished
            else
            {
                Canvas.Hide();
                CharacterManager.Hide();
            }
        }
    }

    /// <summary>
    /// Handles a provided StoryGraph
    /// </summary>
    internal class UVNFStoryManager
    {
        private UVNFManager _manager;
        private UVNFCanvas _canvas;

        private StoryGraph _storyGraph;
        private StoryElement _currentElement;

        private TaskManager.TaskState _currentTask;

        private UVNFStoryManager _subgraphHandler;
        private bool _handlingSubgraph = false;

        private event Action _afterSubgraphHandler;

        private StoryElement _afterSubgraphElement;

        /// <summary>
        /// Creates a StoryManager that automatically starts at the start of the provided Graph
        /// </summary>
        /// <param name="graph"></param>
        /// <param name="manager"></param>
        /// <param name="canvas"></param>
        /// <param name="afterStoryHandler"></param>
        public UVNFStoryManager(StoryGraph graph, UVNFManager manager, UVNFCanvas canvas, Action afterStoryHandler)
        {
            _storyGraph = graph;

            _manager = manager;
            _canvas = canvas;

            if(_storyGraph != null)
                StartStory();

            _afterSubgraphHandler += afterStoryHandler;
        }

        public void CreateSubStory(StoryGraph graph, UVNFManager manager, UVNFCanvas canvas)
        {
            if (_subgraphHandler == null)
            {
                _afterSubgraphElement = _currentElement.Next;

                _subgraphHandler = new UVNFStoryManager(graph, manager, canvas, HandleSubgraphFinish);
                _handlingSubgraph = true;
            }
            else
            {
                _subgraphHandler.CreateSubStory(graph, manager, canvas);
            }
        }

        public void HandleSubgraphFinish()
        {
            _subgraphHandler = null;
            _handlingSubgraph = false;

            AdvanceStory(_afterSubgraphElement);
        }

        #region StoryElements
        public void StartStory()
        {
            _storyGraph.ConnectStoryElements();
            _currentElement = _storyGraph.GetRootStory()[0];

            _currentTask = TaskManager.CreateTask(_currentElement.Execute(_manager, _canvas));
            _currentTask.Finished += AdvanceStory;

            _currentTask.Start();
        }

        public void AdvanceStory(bool manual)
        {
            if (!manual && !_handlingSubgraph)
            {
                if (_currentElement.Next != null && _currentTask != null && !_currentTask.Running)
                {
                    _currentElement = _currentElement.Next;

                    _currentTask = TaskManager.CreateTask(_currentElement.Execute(_manager, _canvas));
                    _currentTask.Finished += AdvanceStory;
                    _currentTask.Start();
                }
                else
                    _afterSubgraphHandler?.Invoke();
            }
        }

        public void AdvanceStory(StoryElement newStoryPoint, bool continueToRun = false)
        {
            if (_handlingSubgraph)
                _subgraphHandler.AdvanceStory(false);
            else if (newStoryPoint != null)
            {
                if(!continueToRun)
                    _currentTask.Stop();

                _currentElement = newStoryPoint;

                _currentTask = TaskManager.CreateTask(_currentElement.Execute(_manager, _canvas));
                _currentTask.Finished += AdvanceStory;
                _currentTask.Start();
            }
            else
                _afterSubgraphHandler?.Invoke();
        }
        #endregion
    }
}