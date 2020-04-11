using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core;
using Entities;

public class GameManager : MonoBehaviour
{
    public Player Player;
    public Location CurrentLocation;

    public UDSFCanvas Canvas;

    public StoryContainer CurrentStory;
    public StoryElement CurrentElement;
    private List<Tuple<string, string>> _storyLog = new List<Tuple<string, string>>();

    private Dictionary<string, object[]> _eventFlags;

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
        //TODO
        //Accept a storycontainer, set the first node as start
    }

    private void AdvanceStory()
    {

    }

    public void LogStoryEvent(string characterName, string text)
    {
        _storyLog.Add(new Tuple<string, string>(characterName, text));
    }
    #endregion
}
