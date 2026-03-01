using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Week", menuName = "Scriptable Objects/Week")]
public class Week : ScriptableObject {
    public int weekNumber;

    public float timeHandSpeed = 0.48f;

    public float firstPreparationTime = 60f;
    public float dailyPreparationTime = 15f;

    public ActivityWithTime[] fixedActivities;

    [Header("Task Lists")]
    public ActivityWithCount[] dailyTasks;
    public ActivityWithCount[] weeklyTasks;
    public ActivityWithCount[] bonusTasks;
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