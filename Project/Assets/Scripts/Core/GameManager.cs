using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core;
using Entities;

public class GameManager : MonoBehaviour
{
    public static GameManager Manager;

    public static Player Player;
    public static Location CurrentLocation;

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
}
