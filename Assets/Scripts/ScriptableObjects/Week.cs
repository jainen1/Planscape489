using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Week", menuName = "Scriptable Objects/Week")]
public class Week : ScriptableObject {
    public MusicType musicType = MusicType.Calm;
    public GameplayType gameplayType = GameplayType.Calendar;

    [Header("Details")]
    public float timeHandSpeed = 0.48f;

    public float firstPreparationTime = 60f;
    public float dailyPreparationTime = 15f;

    public ResourceBarValues.Collection[] resourceBars;

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

[Serializable]
public class ResourceBarValues {
    public float min;
    public float max;

    [Serializable]
    public class Collection {
        public float startingValue;
        public ResourceBarValues[] resourceBars = new ResourceBarValues[0];
    }
}

public enum MusicType {
    Calm,
    Tense,
    SuperTense
}

public enum GameplayType {
    Calendar,
    Invaders,
    Zombies
}