using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Week", menuName = "Scriptable Objects/Week")]
public class Week : ScriptableObject {
    public AudioClip music;

    [Header("Details")]
    public float timeHandSpeed = 0.48f;

    public float firstPreparationTime = 60f;
    public float dailyPreparationTime = 15f;

    public float startingHappiness = 70;
    public float startingMoney = 2000;

    [Header("Activities")]
    public ActivityWithTime[] fixedActivities;
    public ActivityWithCount[] requiredTasks;
    public ActivityWithCount[] bonusTasks;

    [Header("Events")]
    //public float randomEventChance;
    public EventWithTime[] fixedEvents;
}

[Serializable]
public class ActivityWithTime {
    public ActivityObject activity;
    public Vector2 time;
}

[Serializable]
public class ActivityWithCount {
    public ActivityObject activity;
    public int count;
}

[Serializable]
public class EventWithTime {
    public EventObject eventObject;
    public Vector2 time;
}