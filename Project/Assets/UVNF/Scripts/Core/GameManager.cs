using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using CoroutineManager;

public class GameManager : MonoBehaviour
{
    [Header("UDSF Components")]
    public UVNFCanvas Canvas;
    public AudioManager AudioManager;
    public CanvasCharacterManager CharacterManager;

    [Header("Story Settings")]
    public StoryGraph CurrentStory;

    public float TimeoutBeforeStart = 0f;
    private float _timeoutTimer = 0f;

    private StoryElement CurrentElement;
    private TaskManager.TaskState _currentTask;

    [Header("Variables")]
    public VariableManager Variables;

    private List<Tuple<string, string>> _storyLog = new List<Tuple<string, string>>();

    private Dictionary<string, object[]> _eventFlags = new Dictionary<string, object[]>();

    public bool AddEventFlag(string eventFlag, params object[] eventValues)
    {
        if (_eventFlags.ContainsKey(eventFlag))
            return false;
        _eventFlags.Add(eventFlag, eventValues);
        return true;
    }

    public bool ReachedEventFlag(string eventFlag)
    {
        return _eventFlags.ContainsKey(eventFlag);
    }

    public object[] GetEventFlagValues(string eventFlag)
    {
        if (!_eventFlags.ContainsKey(eventFlag))
            return null;
        return _eventFlags[eventFlag];
    }

    #region StoryElements
    public void Awake()
    {
        Canvas.HideLoadScreen();

        CurrentStory.ConnectStoryElements();
        StartStory();
    }

    public void StartStory()
    {
        CurrentStory.ConnectStoryElements();
        CurrentElement = CurrentStory.GetRootStory()[0];

        _currentTask = TaskManager.CreateTask(CurrentElement.Execute(this, Canvas));
        _currentTask.Finished += AdvanceStory;
        _currentTask.Start();
    }

    public void AdvanceStory(bool manual)
    {
        if(!manual)
        {
            if (CurrentElement.Next != null && _currentTask != null && !_currentTask.Running)
            {
                CurrentElement = CurrentElement.Next;

                _currentTask = TaskManager.CreateTask(CurrentElement.Execute(this, Canvas));
                _currentTask.Finished += AdvanceStory;
                _currentTask.Start();
            }
            else
            {
                Debug.Log("Story finished.");
            }
        }
    }

    public void AdvanceStory(StoryElement newStoryPoint)
    {
        if (newStoryPoint != null)
        {
            _currentTask.Stop();
            CurrentElement = newStoryPoint;

            _currentTask = TaskManager.CreateTask(CurrentElement.Execute(this, Canvas));
            _currentTask.Finished += AdvanceStory;
            _currentTask.Start();
        }
        else
        {
            Debug.Log("Story finished.");
        }
    }

    public void LogStoryEvent(string characterName, string text)
    {
        _storyLog.Add(new Tuple<string, string>(characterName, text));
    }
    #endregion
}
