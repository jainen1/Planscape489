using UnityEngine;
using System;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "EventObject", menuName = "Scriptable Objects/EventObject")]
public class EventObject : ScriptableObject
{
    public string title;
    public string description;

    public List<EventChoice> choices;
}

[Serializable]
public class EventChoice {
    public string description;
    public EventResult result;
}

[Serializable]
public class EventResult {
    public int happinessChange;
    public int moneyChange;
    public List<ActivityWithTime> addFixedActivities;
    public List<ActivityWithCountAndType> addActivities;
    public List<EventObject> addEvents;
}

[Serializable]
public class ActivityWithCountAndType {
    public ActivityWithCount activityWithCount;
    public ActivityType activityType;
}