using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Week", menuName = "Scriptable Objects/Week")]
public class Week : ScriptableObject {
    public AudioClip music;

    public float timeHandSpeed = 0.48f;

    public float firstPreparationTime = 60f;
    public float dailyPreparationTime = 15f;

    public float startingHappiness = 70;
    public float startingMoney = 2000;

    public ActivityWithTime[] fixedActivities;

    [Header("Task Lists")]
    public ActivityWithCount[] requiredTasks;
    public ActivityWithCount[] bonusTasks;

    public float randomEventChance;
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